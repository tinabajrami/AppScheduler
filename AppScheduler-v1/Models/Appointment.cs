using System;
using System.ComponentModel.DataAnnotations;
using AppScheduler_v1.Areas.Identity.Data;

namespace AppScheduler_v1.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string PatientId { get; set; }

        [Required]
        public string DoctorId { get; set; }

        [Required]
        public string Symptoms { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public AppointmentTime AppointmentTimeSlot { get; set; }

        [Required]
        public AppUser Patient { get; set; } // Navigation property for Patient

        [Required]
        public AppUser Doctor { get; set; } // Navigation property for Doctor
    }

    public enum AppointmentTime
    {
        [Display(Name = "09:00 - 10:00")]
        TimeSlot1, 

        [Display(Name = "10:00 - 11:00")]
        TimeSlot2, 

        [Display(Name = "11:00 - 12:00")]
        TimeSlot3,

        [Display(Name = "12:00 - 13:00")]
        TimeSlot4,

        [Display(Name = "13:00 - 14:00")]
        TimeSlot5, 

        [Display(Name = "14:00 - 15:00")]
        TimeSlot6,

        [Display(Name = "15:00 - 16:00")]
        TimeSlot7,

        [Display(Name = "16:00 - 17:00")]
        TimeSlot8,

        [Display(Name = "17:00 - 18:00")]
        TimeSlot9, 

    }

    /*public static class TimeSlotHelper
    {
        public static string GetStartTime(AppointmentTime appointmentTime)
        {
            switch (appointmentTime)
            {
                case AppointmentTime.TimeSlot1:
                    return "09:00";
                case AppointmentTime.TimeSlot2:
                    return "10:00";
                case AppointmentTime.TimeSlot3:
                    return "11:00";
                case AppointmentTime.TimeSlot4:
                    return "12:00";
                case AppointmentTime.TimeSlot5:
                    return "13:00";
                case AppointmentTime.TimeSlot6:
                    return "14:00";
                case AppointmentTime.TimeSlot7:
                    return "15:00";
                case AppointmentTime.TimeSlot8:
                    return "16:00";
                case AppointmentTime.TimeSlot9:
                    return "17:00";
                default:
                    return string.Empty;
            }
        }
    }*/
}
