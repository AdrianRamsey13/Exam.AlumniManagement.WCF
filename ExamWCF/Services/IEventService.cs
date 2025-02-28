using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ExamWCF.DTOs;

namespace ExamWCF.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEventService" in both code and config file together.
    [ServiceContract]
    public interface IEventService
    {
        [OperationContract]
        IEnumerable<EventDTO> GetEvents();

        [OperationContract]
        EventDTO GetEventByID(int eventID);

        [OperationContract]
        void InsertEvent(EventDTO eventDTO);

        [OperationContract]
        void UpsertEvent(EventDTO eventDTO);

        [OperationContract]
        void UpdateEvent(EventDTO eventDTO);

        [OperationContract]
        void DeleteEvent(int eventID);
    }
}
