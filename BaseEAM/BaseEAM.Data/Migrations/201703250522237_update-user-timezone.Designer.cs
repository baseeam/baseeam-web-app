/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

// <auto-generated />
namespace BaseEAM.Data.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class updateusertimezone : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(updateusertimezone));
        
        string IMigrationMetadata.Id
        {
            get { return "201703250522237_update-user-timezone"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
