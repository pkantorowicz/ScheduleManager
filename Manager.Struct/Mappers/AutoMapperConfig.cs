using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Manager.Core.Enums;
using Manager.Core.Models;
using Manager.Struct.DTO;

namespace Manager.Struct.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Schedule, ScheduleDto>()
                    .ForMember(vm => vm.Creator,
                        map => map.MapFrom(s => s.Creator.Name))
                    .ForMember(vm => vm.Attendees, map =>
                        map.MapFrom(s => s.Attendees.Select(a => a.UserId)));

                cfg.CreateMap<Schedule, ScheduleDetailsDto>()
                    .ForMember(vm => vm.Creator,
                        map => map.MapFrom(s => s.Creator.Name))
                    .ForMember(vm => vm.Attendees, map =>
                        map.UseValue(new List<UserDto>()))
                    .ForMember(vm => vm.Status, map =>
                        map.MapFrom(s => s.Status.ToString()))
                    .ForMember(vm => vm.Type, map =>
                        map.MapFrom(s => s.Type.ToString()))
                    .ForMember(vm => vm.Statuses, map =>
                        map.UseValue(Enum.GetNames(typeof(ScheduleStatus)).ToArray()))
                    .ForMember(vm => vm.Types, map =>
                        map.UseValue(Enum.GetNames(typeof(ScheduleType)).ToArray()));
                
                cfg.CreateMap<Activity, ActivityDto>()
                    .ForMember(vm => vm.Creator,
                        map => map.MapFrom(t => t.Creator.Name));

                cfg.CreateMap<Activity, ActivityDetailsDto>()
                    .ForMember(vm => vm.Creator,
                        map => map.MapFrom(t => t.Creator.Name))
                    .ForMember(vm => vm.Status, map =>
                        map.MapFrom(t => t.Status.ToString()))
                    .ForMember(vm => vm.Type, map =>
                        map.MapFrom(t => t.Type.ToString()))
                    .ForMember(vm => vm.Priorities, map =>
                        map.MapFrom(t => t.Type.ToString()))
                    .ForMember(vm => vm.Statuses, map =>
                        map.UseValue(Enum.GetNames(typeof(ActivityStatus)).ToArray()))
                    .ForMember(vm => vm.Types, map =>
                        map.UseValue(Enum.GetNames(typeof(ActivityType)).ToArray()))
                    .ForMember(vm => vm.Priorities, map =>
                        map.UseValue(Enum.GetNames(typeof(ActivityPriority)).ToArray()));

                cfg.CreateMap<User, UserDto>()
                    .ForMember(vm => vm.SchedulesCreated,
                        map => map.MapFrom(u => u.SchedulesCreated.Count()));

                cfg.CreateMap<ScheduleDto, Schedule>()                  
                    .ForMember(s => s.Creator, map => map.UseValue((Schedule)null))
                    .ForMember(s => s.Attendees, map => map.UseValue(new List<Attendee>()));

                cfg.CreateMap<ActivityDto, Activity>()                  
                    .ForMember(s => s.Creator, map => map.UseValue((Activity)null));

                cfg.CreateMap<UserDto, User>();
            })
            .CreateMapper();
    }
}