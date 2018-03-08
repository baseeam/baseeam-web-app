/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseEAM.Services
{
    public class ImportResult
    {
        public DateTime? LastRunStartDateTime { get; set; }
        public DateTime? LastRunEndDateTime { get; set; }

        public int TotalRecords { get; set; }
        public int SkippedRecords { get; set; }

        public IList<ImportMessage> Messages { get; private set; }

        public ImportResult()
        {
            this.Messages = new List<ImportMessage>();
        }

        public int Warnings
        {
            get { return Messages != null ? Messages.Count(x => x.MessageType == ImportMessageType.Warning) : 0; }
        }

        public int Errors
        {
            get { return Messages != null ? Messages.Count(x => x.MessageType == ImportMessageType.Error) : 0; }
        }

        public ImportMessage AddInfo(int affectedRowNumber, string message)
        {
            return this.AddMessage(affectedRowNumber, message, ImportMessageType.Info);
        }

        public ImportMessage AddWarning(int affectedRowNumber, string message)
        {
            return this.AddMessage(affectedRowNumber, message, ImportMessageType.Warning);
        }

        public ImportMessage AddError(int affectedRowNumber, string message)
        {
            return this.AddMessage(affectedRowNumber, message, ImportMessageType.Error);
        }

        public ImportMessage AddMessage(int affectedRowNumber, string message, ImportMessageType severity)
        {
            var msg = new ImportMessage(affectedRowNumber, message, severity);
            this.Messages.Add(msg);
            return msg;
        }
     
    }
}
