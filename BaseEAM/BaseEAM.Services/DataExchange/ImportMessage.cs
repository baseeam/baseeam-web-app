/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Services
{
    public class ImportMessage
    {
        public ImportMessage(int affectedRowNumber, string message, ImportMessageType messageType = ImportMessageType.Info)
        {
            this.Message = StringFormat(affectedRowNumber, message);
            this.MessageType = messageType;
            this.AffectedRowNumber = affectedRowNumber;
        }

        public ImportMessageType MessageType { get; set; }
        public string Message { get; set; }

        public int AffectedRowNumber { get; set; }


        private string StringFormat(int affectedRowNumber, string message)
        {
            var result = message;

            string appendix = null;

            if (affectedRowNumber != 0)
                appendix = "Pos: " + affectedRowNumber.ToString();

            if (!string.IsNullOrEmpty(appendix))
                result = string.Format("{0} [{1}]", result, appendix);

            return result;
        }

    }

    public enum ImportMessageType
    {
        Info = 0,
        Warning = 5,
        Error = 10
    }
}
