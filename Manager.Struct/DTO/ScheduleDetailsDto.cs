using System;
using System.Collections.Generic;

namespace Manager.Struct.DTO
{
    public class ScheduleDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Creator { get; set; }
        public int CreatorId { get; set; }
        public ICollection<UserDto> Attendees { get; set; }
        
        public string[] Statuses { get; set; }
        public string[] Types { get; set; }
    }
}
