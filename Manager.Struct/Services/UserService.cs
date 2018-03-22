using AutoMapper;
using Manager.Core.Models;
using Manager.Core.Repositories;
using Manager.Struct.DTO;
using System.Collections.Generic;
using Manager.Struct.Exceptions;
using System.Threading.Tasks;
using Manager.Core.Queries.Users;
using Manager.Core.Types;

namespace Manager.Struct.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IAttendeeRepository _attendeeRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IScheduleRepository scheduleRepository,
            IAttendeeRepository attendeeRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _scheduleRepository = scheduleRepository;
            _attendeeRepository = attendeeRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> GetAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);
            return _mapper.Map<User, UserDto>(user);
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetSingleAsync(u => u.Email == email);
            return _mapper.Map<User, UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
        }

        public async Task<PagedResult<UserDto>> BrowseAsync()
        {
            var users = await _userRepository.GetAllPageable();
            return _mapper.Map<PagedResult<User>, PagedResult<UserDto>>(users);
        }

        public async Task<PagedResult<UserDto>> BrowseByProfessionAsync(BrowseUsersByProfession query)
        {
            var filterUsers = await _userRepository.GetAllPageable(u => u.Profession == query.Profession, query);
            return _mapper.Map<PagedResult<User>, PagedResult<UserDto>>(filterUsers);
        }

        public async Task<PagedResult<UserDto>> BrowseByRoleAsync(BrowseUsersByRole query)
        {
            var filterUsers = await _userRepository.GetAllPageable(u => u.Role == query.Role, query);
            return _mapper.Map<PagedResult<User>, PagedResult<UserDto>>(filterUsers);
        }

        public async Task UpdateUserAsync(int id, string name, string email, string fullName,
           string avatar, string role, string profession)
        {
           var user = await _userRepository.GetAsync(id);
           if (user == null)
           {
               throw new ServiceException(ErrorCodes.InvalidName,
                   $"User with id: {name} not exists.");
           }

           user.SetName(name);
           user.SetEmail(email);
           user.SetFullName(fullName);
           user.SetAvatar(avatar);
           user.SetRole(role);
           user.SetProfession(profession);          

           await _userRepository.UpdateAsync(user);
        }      

        public async Task RemoveUserScheduleAsync(int id)
        {
            var schedules = await _scheduleRepository.FindByAsync(s => s.CreatorId == id);

            foreach (var schedule in schedules)
            {
                _attendeeRepository.DeleteWhereAsync(a => a.Id == id);
                _scheduleRepository.DeleteAsync(schedule);
            }

            await _userRepository.Commit();
        }

        public async Task RemoveUserAttendeeAsync(int id)
        {
            var attendees = await _attendeeRepository.FindByAsync(a => a.Id == id);

            foreach (var attendee in attendees)
            {
                _attendeeRepository.DeleteAsync(attendee);
            }

            await _userRepository.Commit();
        }

        public async Task RemoveUserAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound,
                    $"User with id: {id} not exists.");
            }

            await RemoveUserAttendeeAsync(id);
            await RemoveUserScheduleAsync(id);

             _userRepository.DeleteAsync(user);

            await _userRepository.Commit();
        }
    }
}