# SMART BURGUER VALUE - SISTEMA DE PRECIFICAÇÃO PARA HAMBURGUERIA

Sistema desenvolvido para gerenciamento financeiro e precificação de hamburguerias


##Código Db Diagram
Table Enterprise {
  Id Guid [pk, increment]
  Name varchar
  CNPJ varchar
  PhoneNumber varchar
  State varchar
  City varchar
  Street varchar
  CEP varchar
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
  Plan varchar
}


Table User {
  Id Guid [pk, increment]
  Name varchar
  Email varchar [unique]
  PasswordHash varchar
  Role varchar
  EnterpriseId Guid [ref: > Enterprise.Id]
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table UnityType {
  Id Guid [pk, increment]
  Name varchar
  Symbol varchar
  BaseUnit varchar
  ConversionFactor decimal
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table Ingredient {
  Id Guid [pk, increment]
  Name varchar
  PurchaseQuantity decimal
  UnitOfMeasureId Guid [ref: > UnityType.Id]
  PurchasePrice decimal
  PurchaseDate datetime
  EnterpriseId Guid [ref: > Enterprise.Id]
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table Product {
  Id Guid [pk, increment]
  Name varchar
  Description text
  EnterpriseId Guid [ref: > Enterprise.Id]
  ImageUrl varchar
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table ProductIngredient {
  Id Guid [pk, increment]
  ProductId Guid [ref: > Product.Id]
  IngredientId Guid [ref: > Ingredient.Id]
  QuantityUsedInBase decimal
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table Combo {
  Id Guid [pk, increment]
  Name varchar
  Description text
  EnterpriseId Guid [ref: > Enterprise.Id]
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table ComboProduct {
  Id Guid [pk, increment]
  ComboId Guid [ref: > Combo.Id]
  ProductId Guid [ref: > Product.Id]
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table FixedCost {
  Id Guid [pk, increment]
  Name varchar
  Description varchar
  Value decimal
  EnterpriseId Guid [ref: > Enterprise.Id]
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table SalesGoal {
  Id Guid [pk, increment]
  Description varchar
  GoalValue decimal
  StartDate date
  EndDate date
  EntepriseId Guid [ref: > Enterprise.Id]
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table DailyEntry {
  Id Guid [pk, increment]
  EntryDate date
  Revenue decimal
  Notes text
  EntepriseId Guid [ref: > Enterprise.Id]
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table Employee {
  Id Guid [pk, increment]
  Name varchar
  Position varchar
  EmploymentType varchar
  MonthlySalary decimal
  EnterpriseId Guid [ref: > Enterprise.Id]
  UserId Guid [ref: > User.Id]
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table EmployeeWorkSchedule {
  Id Guid [pk, increment]
  EmployeeId Guid [ref: > Employee.Id]
  Weekday varchar
  DailyRate decimal
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}




Table Purchase {
  Id Guid [pk, increment]
  SupplierName varchar
  PurchaseDate datetime
  TotalAmount decimal
  EnteprriseId Guid [ref: > Enterprise.Id]
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table PurchaseItem {
  Id Guid [pk, increment]
  PurchaseId Guid [ref: > Purchase.Id]
  IngredientId Guid [ref: > Ingredient.Id]
  Quantity decimal
  UnitPrice decimal
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table Promotion {
  Id Guid [pk, increment]
  Title varchar
  Description text
  DiscountPercent decimal
  StartDate datetime
  EndDate datetime
  EnterpriseId Guid [ref: > Enterprise.Id]
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}


Table FinancialSnapshot {
  Id Guid [pk, increment]
  BusinessId Guid [ref: > Enterprise.Id]
  SnapshotDate date
  TotalRevenue decimal
  TotalCost decimal
  GrossProfit decimal
  Markup decimal
  Margin decimal
  CPV decimal
  CreatedAt datetime
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}
Table ProductCostAnalysis {
  Id Guid [pk, increment]
  ProductId Guid [ref: > Product.Id]
  BusinessId Guid [ref: > Enterprise.Id]
  AnalysisDate date
  UnitCost decimal
  SellingPrice decimal
  Markup decimal
  Margin decimal
  CPV decimal
  DateCreated datetime
  DateUpdated datetime
  IsActive boolean
}







