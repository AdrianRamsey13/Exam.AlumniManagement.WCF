using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ExamWCF.DTOs;
using ExamWCF.Entities;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EventService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EventService.svc or EventService.svc.cs at the Solution Explorer and start debugging.
    public class EventService : IEventService
    {
        private AlumniManagementDataContext _dataContext;
        public string connectionStringSettings = ConfigurationManager.ConnectionStrings["KDP22ConnectionString"].ToString();

        public EventService()
        {
            _dataContext = new AlumniManagementDataContext(connectionStringSettings);
        }

        public IEnumerable<EventDTO> GetEvents()
        {
            var query = from e in _dataContext.Events
                        select new EventDTO
                        {
                            EventID = e.EventID,
                            Title = e.Title,
                            Description = e.Description,
                            StartDate = e.StartDate,
                            EndDate = e.EndDate,
                            EventImagePath = e.EventImagePath,
                            EventImageName = e.EventImageName,
                            ModifiedDate = e.ModifiedDate,
                            IsClosed = e.IsClosed                            
                        };
            return query.ToList().OrderByDescending(e => e.ModifiedDate);
        }

        public EventDTO GetEventByID(int eventID)
        {
           var searchEvent = GetEvents().FirstOrDefault(e => e.EventID == eventID);
            return searchEvent;
        }

        public void InsertEvent(EventDTO eventDTO)
        {
            var events = Mapping.Mapper.Map<Event>(eventDTO);
            events.ModifiedDate = DateTime.Now;
            _dataContext.Events.InsertOnSubmit(events);
            _dataContext.SubmitChanges();
        }

        public void UpsertEvent(EventDTO eventDto)
        {
            try
            {
                using (var connection = new SqlConnection(_dataContext.Connection.ConnectionString))
                {
                    using (var command = new SqlCommand("dbo.UpsertEvent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@EventID", SqlDbType.Int) { Value = (object)eventDto.EventID ?? 0 });
                        command.Parameters.Add(new SqlParameter("@Title", SqlDbType.VarChar, 100) { Value = eventDto.Title });
                        command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 255) { Value = eventDto.Description });
                        command.Parameters.Add(new SqlParameter("@EventImagePath", SqlDbType.VarChar, 255) { Value = eventDto.EventImagePath });
                        command.Parameters.Add(new SqlParameter("@EventImageName", SqlDbType.VarChar, 50) { Value = eventDto.EventImageName });
                        command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Date) { Value = eventDto.StartDate });
                        command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.Date) { Value = eventDto.EndDate });
                        command.Parameters.Add(new SqlParameter("@IsClosed", SqlDbType.Bit) { Value = eventDto.IsClosed });

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void UpdateEvent(EventDTO eventDTO)
        {
            var existingEvent = _dataContext.Events.FirstOrDefault(r => r.EventID == eventDTO.EventID);
            Mapping.Mapper.Map(eventDTO, existingEvent);
            existingEvent.ModifiedDate = DateTime.Now;
            _dataContext.SubmitChanges();
        }

        public void DeleteEvent(int eventID)
        {
            var result = _dataContext.Events.FirstOrDefault(r => r.EventID == eventID);
            _dataContext.Events.DeleteOnSubmit(result);
            _dataContext.SubmitChanges();
        }
    }
}
