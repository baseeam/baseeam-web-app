namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedata : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SLAInstanceTerm",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SLAInstanceId = c.Long(),
                        SLATermId = c.Long(),
                        TrackingBaseDateTime = c.DateTime(precision: 0),
                        TrackingDateTime = c.DateTime(precision: 0),
                        Violated = c.Boolean(nullable: false),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SLAInstance",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EntityId = c.Long(),
                        EntityType = c.String(maxLength: 64, storeType: "nvarchar"),
                        SLADefinitionId = c.Long(),
                        Violated = c.Boolean(nullable: false),
                        Closed = c.Boolean(nullable: false),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SLADefinition",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        EntityType = c.String(maxLength: 64, storeType: "nvarchar"),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SLATerm",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SLADefinitionId = c.Long(),
                        TrackingBaseField = c.String(maxLength: 64, storeType: "nvarchar"),
                        TrackingField = c.String(maxLength: 64, storeType: "nvarchar"),
                        LimitHours = c.Int(nullable: false),
                        LimitMinutes = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotificationSequence",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SLATermId = c.Long(),
                        Sequence = c.Int(nullable: false),
                        SendingTimeHours = c.Int(nullable: false),
                        SendingTimeMinutes = c.Int(nullable: false),
                        Users = c.String(maxLength: 512, storeType: "nvarchar"),
                        MessageTemplate = c.String(maxLength: 128, storeType: "nvarchar"),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequestForQuotationVendorItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RequestForQuotationVendorId = c.Long(),
                        RequestForQuotationItemId = c.Long(),
                        QuantityQuoted = c.Decimal(precision: 19, scale: 4),
                        UnitPriceQuoted = c.Decimal(precision: 19, scale: 4),
                        SubtotalQuoted = c.Decimal(precision: 19, scale: 4),
                        IsAwarded = c.Boolean(nullable: false),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequestForQuotationVendor",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RequestForQuotationId = c.Long(),
                        VendorId = c.Long(),
                        VendorContactName = c.String(maxLength: 64, storeType: "nvarchar"),
                        VendorContactEmail = c.String(maxLength: 64, storeType: "nvarchar"),
                        VendorContactPhone = c.String(maxLength: 64, storeType: "nvarchar"),
                        VendorContactFax = c.String(maxLength: 64, storeType: "nvarchar"),
                        VendorQuoteNumber = c.String(maxLength: 64, storeType: "nvarchar"),
                        VendorQuoteDate = c.DateTime(precision: 0),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequestForQuotationItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RequestForQuotationId = c.Long(),
                        Sequence = c.Int(),
                        ItemId = c.Long(),
                        QuantityRequested = c.Decimal(precision: 19, scale: 4),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequestForQuotation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.Long(),
                        PurchaseRequestId = c.Long(),
                        RequestorId = c.Long(),
                        ExpectedQuoteDate = c.DateTime(precision: 0),
                        DateRequired = c.DateTime(precision: 0),
                        ShipToAddressId = c.Long(),
                        IsSent = c.Boolean(nullable: false),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Priority = c.Int(),
                        AssignmentId = c.Long(),
                        CreatedUserId = c.Long(),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseOrderMiscCost",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PurchaseOrderId = c.Long(),
                        POMiscCostTypeId = c.Long(),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Amount = c.Decimal(precision: 19, scale: 4),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseOrderItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PurchaseOrderId = c.Long(),
                        Sequence = c.Int(),
                        ItemId = c.Long(),
                        QuantityOrdered = c.Decimal(precision: 19, scale: 4),
                        QuantityReceived = c.Decimal(precision: 19, scale: 4),
                        UnitPrice = c.Decimal(precision: 19, scale: 4),
                        ReceiveToStoreId = c.Long(),
                        ReceiveToStoreLocatorId = c.Long(),
                        TaxRate = c.Decimal(precision: 19, scale: 4),
                        TaxAmount = c.Decimal(precision: 19, scale: 4),
                        Subtotal = c.Decimal(precision: 19, scale: 4),
                        SubtotalWithTax = c.Decimal(precision: 19, scale: 4),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseOrder",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.Long(),
                        PurchaseRequestId = c.Long(),
                        RequestForQuotationId = c.Long(),
                        RequestForQuotationVendorId = c.Long(),
                        RequestorId = c.Long(),
                        SupervisorId = c.Long(),
                        ExpectedDeliveryDate = c.DateTime(precision: 0),
                        DateOrdered = c.DateTime(precision: 0),
                        DateRequired = c.DateTime(precision: 0),
                        ShipToAddressId = c.Long(),
                        BillToAddressId = c.Long(),
                        VendorId = c.Long(),
                        PaymentTermId = c.Long(),
                        ContractId = c.Long(),
                        IsSent = c.Boolean(nullable: false),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Priority = c.Int(),
                        AssignmentId = c.Long(),
                        CreatedUserId = c.Long(),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AutomatedAction",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        EntityType = c.String(maxLength: 64, storeType: "nvarchar"),
                        WhenUsed = c.Int(),
                        Expression = c.String(maxLength: 128, storeType: "nvarchar"),
                        TriggerType = c.Int(),
                        HoursAfter = c.Int(nullable: false),
                        RepeatCount = c.Int(nullable: false),
                        RepeatInterval = c.Int(nullable: false),
                        CronExpression = c.String(maxLength: 64, storeType: "nvarchar"),
                        ActionTypeId = c.Long(),
                        Users = c.String(maxLength: 512, storeType: "nvarchar"),
                        MessageTemplate = c.String(maxLength: 128, storeType: "nvarchar"),
                        WorkflowDefinitionId = c.Long(),
                        SetExpression = c.String(maxLength: 512, storeType: "nvarchar"),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TenantPaymentCollection",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantPaymentId = c.Long(),
                        ReceivedDate = c.DateTime(precision: 0),
                        ReceivedAmount = c.Decimal(precision: 19, scale: 4),
                        CheckNum = c.String(maxLength: 128, storeType: "nvarchar"),
                        PaymentMethodId = c.Long(),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TenantPayment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.Long(),
                        TenantId = c.Long(),
                        PropertyId = c.Long(),
                        TenantLeaseId = c.Long(),
                        TenantLeasePaymentScheduleId = c.Long(),
                        TenantLeaseChargeId = c.Long(),
                        DueDate = c.DateTime(precision: 0),
                        ChargeTypeId = c.Long(),
                        DueAmount = c.Decimal(precision: 19, scale: 4),
                        DaysLateCount = c.Int(nullable: false),
                        LateFeeAmount = c.Decimal(precision: 19, scale: 4),
                        CollectedAmount = c.Decimal(precision: 19, scale: 4),
                        BalanceAmount = c.Decimal(precision: 19, scale: 4),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TenantLeasePaymentSchedule",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantLeaseId = c.Long(),
                        DueAmount = c.Decimal(precision: 19, scale: 4),
                        DueDate = c.DateTime(precision: 0),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TenantLeaseCharge",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantLeaseId = c.Long(),
                        ChargeTypeId = c.Long(),
                        ChargeAmount = c.Decimal(precision: 19, scale: 4),
                        AmountIsOverridable = c.Boolean(nullable: false),
                        ChargeDueType = c.Int(),
                        ChargeDueDate = c.DateTime(precision: 0),
                        ChargeDueDay = c.Int(),
                        ValidFrom = c.DateTime(precision: 0),
                        ValidTo = c.DateTime(precision: 0),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Property",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.Long(),
                        LocationId = c.Long(),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TenantLease",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.Long(),
                        TenantId = c.Long(),
                        PropertyId = c.Long(),
                        TermStartDate = c.DateTime(precision: 0),
                        TermEndDate = c.DateTime(precision: 0),
                        TermNumber = c.Int(),
                        TermPeriod = c.Int(),
                        TermIsMonthToMonth = c.Boolean(nullable: false),
                        TermRentAmount = c.Decimal(precision: 19, scale: 4),
                        DueFrequency = c.Int(),
                        BiMonthlyStart = c.Int(),
                        BiMonthlyEnd = c.Int(),
                        FirstPaymentDate = c.DateTime(precision: 0),
                        LateFeeEnabled = c.Boolean(nullable: false),
                        GracePeriodDays = c.Int(),
                        LateFeeOption = c.Int(),
                        FlatFee = c.Decimal(precision: 19, scale: 4),
                        BaseAmountPerDay = c.Decimal(precision: 19, scale: 4),
                        AmountPerDay = c.Decimal(precision: 19, scale: 4),
                        PercentOfRentPerDay = c.Decimal(precision: 19, scale: 4),
                        StopPerDay = c.Boolean(nullable: false),
                        MaxLateFee = c.Decimal(precision: 19, scale: 4),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Priority = c.Int(),
                        AssignmentId = c.Long(),
                        CreatedUserId = c.Long(),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tenant",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AddressId = c.Long(),
                        UserId = c.Long(),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseRequestItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PurchaseRequestId = c.Long(),
                        Sequence = c.Int(),
                        ItemId = c.Long(),
                        QuantityRequested = c.Decimal(precision: 19, scale: 4),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseRequest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.Long(),
                        RequestorId = c.Long(),
                        DateRequired = c.DateTime(precision: 0),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Priority = c.Int(),
                        AssignmentId = c.Long(),
                        CreatedUserId = c.Long(),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Receipt", "PurchaseOrderId", c => c.Long());
            AddColumn("dbo.StoreItem", "PurchaseRequestId", c => c.Long());
            AddColumn("dbo.Contact", "TenantId", c => c.Long());
            CreateIndex("dbo.SLAInstanceTerm", "SLATermId");
            CreateIndex("dbo.SLAInstanceTerm", "SLAInstanceId");
            CreateIndex("dbo.SLAInstance", "SLADefinitionId");
            CreateIndex("dbo.SLATerm", "SLADefinitionId");
            CreateIndex("dbo.NotificationSequence", "SLATermId");
            CreateIndex("dbo.RequestForQuotationVendorItem", "RequestForQuotationItemId");
            CreateIndex("dbo.RequestForQuotationVendorItem", "RequestForQuotationVendorId");
            CreateIndex("dbo.RequestForQuotationVendor", "VendorId");
            CreateIndex("dbo.RequestForQuotationVendor", "RequestForQuotationId");
            CreateIndex("dbo.RequestForQuotationItem", "ItemId");
            CreateIndex("dbo.RequestForQuotationItem", "RequestForQuotationId");
            CreateIndex("dbo.RequestForQuotation", "AssignmentId");
            CreateIndex("dbo.RequestForQuotation", "ShipToAddressId");
            CreateIndex("dbo.RequestForQuotation", "RequestorId");
            CreateIndex("dbo.RequestForQuotation", "PurchaseRequestId");
            CreateIndex("dbo.RequestForQuotation", "SiteId");
            CreateIndex("dbo.PurchaseOrderMiscCost", "POMiscCostTypeId");
            CreateIndex("dbo.PurchaseOrderMiscCost", "PurchaseOrderId");
            CreateIndex("dbo.PurchaseOrderItem", "ReceiveToStoreLocatorId");
            CreateIndex("dbo.PurchaseOrderItem", "ReceiveToStoreId");
            CreateIndex("dbo.PurchaseOrderItem", "ItemId");
            CreateIndex("dbo.PurchaseOrderItem", "PurchaseOrderId");
            CreateIndex("dbo.PurchaseOrder", "AssignmentId");
            CreateIndex("dbo.PurchaseOrder", "ContractId");
            CreateIndex("dbo.PurchaseOrder", "PaymentTermId");
            CreateIndex("dbo.PurchaseOrder", "VendorId");
            CreateIndex("dbo.PurchaseOrder", "BillToAddressId");
            CreateIndex("dbo.PurchaseOrder", "ShipToAddressId");
            CreateIndex("dbo.PurchaseOrder", "SupervisorId");
            CreateIndex("dbo.PurchaseOrder", "RequestorId");
            CreateIndex("dbo.PurchaseOrder", "RequestForQuotationVendorId");
            CreateIndex("dbo.PurchaseOrder", "RequestForQuotationId");
            CreateIndex("dbo.PurchaseOrder", "PurchaseRequestId");
            CreateIndex("dbo.PurchaseOrder", "SiteId");
            CreateIndex("dbo.Receipt", "PurchaseOrderId");
            CreateIndex("dbo.AutomatedAction", "WorkflowDefinitionId");
            CreateIndex("dbo.AutomatedAction", "ActionTypeId");
            CreateIndex("dbo.TenantPaymentCollection", "PaymentMethodId");
            CreateIndex("dbo.TenantPaymentCollection", "TenantPaymentId");
            CreateIndex("dbo.TenantPayment", "ChargeTypeId");
            CreateIndex("dbo.TenantPayment", "TenantLeaseChargeId");
            CreateIndex("dbo.TenantPayment", "TenantLeasePaymentScheduleId");
            CreateIndex("dbo.TenantPayment", "TenantLeaseId");
            CreateIndex("dbo.TenantPayment", "PropertyId");
            CreateIndex("dbo.TenantPayment", "TenantId");
            CreateIndex("dbo.TenantPayment", "SiteId");
            CreateIndex("dbo.TenantLeasePaymentSchedule", "TenantLeaseId");
            CreateIndex("dbo.TenantLeaseCharge", "ChargeTypeId");
            CreateIndex("dbo.TenantLeaseCharge", "TenantLeaseId");
            CreateIndex("dbo.Property", "LocationId");
            CreateIndex("dbo.Property", "SiteId");
            CreateIndex("dbo.TenantLease", "AssignmentId");
            CreateIndex("dbo.TenantLease", "PropertyId");
            CreateIndex("dbo.TenantLease", "TenantId");
            CreateIndex("dbo.TenantLease", "SiteId");
            CreateIndex("dbo.Tenant", "UserId");
            CreateIndex("dbo.Tenant", "AddressId");
            CreateIndex("dbo.PurchaseRequestItem", "ItemId");
            CreateIndex("dbo.PurchaseRequestItem", "PurchaseRequestId");
            CreateIndex("dbo.PurchaseRequest", "AssignmentId");
            CreateIndex("dbo.PurchaseRequest", "RequestorId");
            CreateIndex("dbo.PurchaseRequest", "SiteId");
            CreateIndex("dbo.StoreItem", "PurchaseRequestId");
            CreateIndex("dbo.Contact", "TenantId");
            AddForeignKey("dbo.SLAInstanceTerm", "SLATermId", "dbo.SLATerm", "Id");
            AddForeignKey("dbo.SLAInstanceTerm", "SLAInstanceId", "dbo.SLAInstance", "Id");
            AddForeignKey("dbo.SLAInstance", "SLADefinitionId", "dbo.SLADefinition", "Id");
            AddForeignKey("dbo.NotificationSequence", "SLATermId", "dbo.SLATerm", "Id");
            AddForeignKey("dbo.SLATerm", "SLADefinitionId", "dbo.SLADefinition", "Id");
            AddForeignKey("dbo.Receipt", "PurchaseOrderId", "dbo.PurchaseOrder", "Id");
            AddForeignKey("dbo.PurchaseOrder", "VendorId", "dbo.Company", "Id");
            AddForeignKey("dbo.PurchaseOrder", "SupervisorId", "dbo.User", "Id");
            AddForeignKey("dbo.PurchaseOrder", "SiteId", "dbo.Site", "Id");
            AddForeignKey("dbo.PurchaseOrder", "ShipToAddressId", "dbo.Address", "Id");
            AddForeignKey("dbo.PurchaseOrder", "RequestorId", "dbo.User", "Id");
            AddForeignKey("dbo.PurchaseOrder", "RequestForQuotationVendorId", "dbo.RequestForQuotationVendor", "Id");
            AddForeignKey("dbo.PurchaseOrder", "RequestForQuotationId", "dbo.RequestForQuotation", "Id");
            AddForeignKey("dbo.RequestForQuotation", "SiteId", "dbo.Site", "Id");
            AddForeignKey("dbo.RequestForQuotation", "ShipToAddressId", "dbo.Address", "Id");
            AddForeignKey("dbo.RequestForQuotation", "RequestorId", "dbo.User", "Id");
            AddForeignKey("dbo.RequestForQuotationVendor", "VendorId", "dbo.Company", "Id");
            AddForeignKey("dbo.RequestForQuotationVendorItem", "RequestForQuotationVendorId", "dbo.RequestForQuotationVendor", "Id");
            AddForeignKey("dbo.RequestForQuotationVendorItem", "RequestForQuotationItemId", "dbo.RequestForQuotationItem", "Id");
            AddForeignKey("dbo.RequestForQuotationVendor", "RequestForQuotationId", "dbo.RequestForQuotation", "Id");
            AddForeignKey("dbo.RequestForQuotationItem", "RequestForQuotationId", "dbo.RequestForQuotation", "Id");
            AddForeignKey("dbo.RequestForQuotationItem", "ItemId", "dbo.Item", "Id");
            AddForeignKey("dbo.RequestForQuotation", "PurchaseRequestId", "dbo.PurchaseRequest", "Id");
            AddForeignKey("dbo.RequestForQuotation", "AssignmentId", "dbo.Assignment", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PurchaseOrder", "PurchaseRequestId", "dbo.PurchaseRequest", "Id");
            AddForeignKey("dbo.PurchaseOrderMiscCost", "PurchaseOrderId", "dbo.PurchaseOrder", "Id");
            AddForeignKey("dbo.PurchaseOrderMiscCost", "POMiscCostTypeId", "dbo.ValueItem", "Id");
            AddForeignKey("dbo.PurchaseOrderItem", "ReceiveToStoreLocatorId", "dbo.StoreLocator", "Id");
            AddForeignKey("dbo.PurchaseOrderItem", "ReceiveToStoreId", "dbo.Store", "Id");
            AddForeignKey("dbo.PurchaseOrderItem", "PurchaseOrderId", "dbo.PurchaseOrder", "Id");
            AddForeignKey("dbo.PurchaseOrderItem", "ItemId", "dbo.Item", "Id");
            AddForeignKey("dbo.PurchaseOrder", "PaymentTermId", "dbo.ValueItem", "Id");
            AddForeignKey("dbo.PurchaseOrder", "ContractId", "dbo.Contract", "Id");
            AddForeignKey("dbo.PurchaseOrder", "BillToAddressId", "dbo.Address", "Id");
            AddForeignKey("dbo.PurchaseOrder", "AssignmentId", "dbo.Assignment", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AutomatedAction", "WorkflowDefinitionId", "dbo.WorkflowDefinition", "Id");
            AddForeignKey("dbo.AutomatedAction", "ActionTypeId", "dbo.ValueItem", "Id");
            AddForeignKey("dbo.Contact", "TenantId", "dbo.Tenant", "Id");
            AddForeignKey("dbo.Tenant", "UserId", "dbo.User", "Id");
            AddForeignKey("dbo.TenantPaymentCollection", "TenantPaymentId", "dbo.TenantPayment", "Id");
            AddForeignKey("dbo.TenantPaymentCollection", "PaymentMethodId", "dbo.ValueItem", "Id");
            AddForeignKey("dbo.TenantPayment", "TenantLeasePaymentScheduleId", "dbo.TenantLeasePaymentSchedule", "Id");
            AddForeignKey("dbo.TenantPayment", "TenantLeaseChargeId", "dbo.TenantLeaseCharge", "Id");
            AddForeignKey("dbo.TenantPayment", "TenantLeaseId", "dbo.TenantLease", "Id");
            AddForeignKey("dbo.TenantPayment", "TenantId", "dbo.Tenant", "Id");
            AddForeignKey("dbo.TenantPayment", "SiteId", "dbo.Site", "Id");
            AddForeignKey("dbo.TenantPayment", "PropertyId", "dbo.Property", "Id");
            AddForeignKey("dbo.TenantPayment", "ChargeTypeId", "dbo.ValueItem", "Id");
            AddForeignKey("dbo.TenantLeasePaymentSchedule", "TenantLeaseId", "dbo.TenantLease", "Id");
            AddForeignKey("dbo.TenantLeaseCharge", "TenantLeaseId", "dbo.TenantLease", "Id");
            AddForeignKey("dbo.TenantLeaseCharge", "ChargeTypeId", "dbo.ValueItem", "Id");
            AddForeignKey("dbo.TenantLease", "TenantId", "dbo.Tenant", "Id");
            AddForeignKey("dbo.TenantLease", "SiteId", "dbo.Site", "Id");
            AddForeignKey("dbo.TenantLease", "PropertyId", "dbo.Property", "Id");
            AddForeignKey("dbo.Property", "SiteId", "dbo.Site", "Id");
            AddForeignKey("dbo.Property", "LocationId", "dbo.Location", "Id");
            AddForeignKey("dbo.TenantLease", "AssignmentId", "dbo.Assignment", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tenant", "AddressId", "dbo.Address", "Id");
            AddForeignKey("dbo.StoreItem", "PurchaseRequestId", "dbo.PurchaseRequest", "Id");
            AddForeignKey("dbo.PurchaseRequest", "SiteId", "dbo.Site", "Id");
            AddForeignKey("dbo.PurchaseRequest", "RequestorId", "dbo.User", "Id");
            AddForeignKey("dbo.PurchaseRequestItem", "PurchaseRequestId", "dbo.PurchaseRequest", "Id");
            AddForeignKey("dbo.PurchaseRequestItem", "ItemId", "dbo.Item", "Id");
            AddForeignKey("dbo.PurchaseRequest", "AssignmentId", "dbo.Assignment", "Id", cascadeDelete: true);
        }
    }
}
