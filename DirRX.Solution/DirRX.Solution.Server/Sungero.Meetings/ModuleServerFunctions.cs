using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace DirRX.Solution.Module.Meetings.Server
{
  partial class ModuleFunctions
  {
    #region Функции для сервиса интеграции
    /// <summary>
    /// Создать совещание
    /// </summary>
    /// <param name="meetingName">Тема совещания</param>
    /// <param name="meetingDate">Дата совещания</param>
    /// <param name="meetingDuration">Длительность совещания</param>
    /// <param name="addMeetingMembers">Список ID участников  совещания</param>
    [Public(WebApiRequestType = RequestType.Post)]
    public void CreateNewMeeting(string meetingName, DateTime meetingDate, double meetingDuration, List<long> addMeetingMembers)
    {
      Logger.Debug("CreateNewMeeting: execution start");
      var meeting = Sungero.Meetings.Meetings.Create();
      
      meeting.Name = meetingName;
      meeting.DateTime = meetingDate;
      meeting.Duration = meetingDuration;
      
      var members = new List<Sungero.CoreEntities.IRecipient>();
      
      foreach (var memberId in addMeetingMembers)
      {
        var member = Sungero.CoreEntities.Recipients.GetAll(r => r.Id == memberId).FirstOrDefault();
        
        if (member != null)
        {
          if (!meeting.Members.Where(m => m.Member == member).Any())
          {
            var memberRow = meeting.Members.AddNew();
            memberRow.Member = member;
          }
        }
      }
      
      try
      {
        meeting.Save();
        Logger.DebugFormat("CreateNewMeeting: meeting with ID {0} has been successfully set up", meeting.Id);
      }
      catch(Exception ex)
      {
        throw AppliedCodeException.Create(String.Format("Failed to create meeting {0}", meetingName),ex);
      }
    }
    #endregion
  }
}