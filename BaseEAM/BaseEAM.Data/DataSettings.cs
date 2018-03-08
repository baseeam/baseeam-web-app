/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Configuration;

namespace BaseEAM.Data
{
    public static class DataSettings
    {
        public static string DataProvider = ConfigurationManager.ConnectionStrings["BaseEAM"].ProviderName;
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["BaseEAM"].ConnectionString;
    }
}
