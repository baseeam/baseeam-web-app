/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using HandlebarsDotNet;

namespace BaseEAM.Services
{
    public class Template
    {
        public static string Render(string source, object data)
        {
            string result = "";
            if(!string.IsNullOrEmpty(source))
            {
                var template = Handlebars.Compile(source);
                result = template(data);
            }

            return result;
        }

        public static string Render(string source, BaseEntity entity, object param)
        {
            string result = "";
            if (!string.IsNullOrEmpty(source))
            {
                var template = Handlebars.Compile(source);
                var data = new
                {
                    entity = entity,
                    param = param
                };
                result = template(data);
            }

            return result;
        }
    }
}
