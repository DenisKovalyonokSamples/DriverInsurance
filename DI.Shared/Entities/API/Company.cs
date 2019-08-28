using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class Company
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("company_type")]
        public string Type { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("shortname")]
        public string ShortName { get; set; }

        [JsonProperty("fullname")]
        public string FullName { get; set; }

        [JsonProperty("kpp")]
        public string Kpp { get; set; }

        [JsonProperty("okpo")]
        public string Okpo { get; set; }

        [JsonProperty("ogrn")]
        public string Ogrn { get; set; }

        [JsonProperty("contact_data")]
        public string ContactData { get; set; }

        [JsonProperty("bank_details")]
        public string BankDetails { get; set; }

        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }

        [JsonProperty("middlename")]
        public string MiddleName { get; set; }

        [JsonProperty("dob")]
        public DateTime? Dob { get; set; }

        [JsonProperty("citizenship")]
        public string Citizenship { get; set; }

        [JsonProperty("pob")]
        public string Pob { get; set; }

        [JsonProperty("sex")]
        public string Sex { get; set; }

        [JsonProperty("document_type")]
        public string DocumentType { get; set; }

        [JsonProperty("document_series")]
        public string DocumentSeries { get; set; }

        [JsonProperty("document_number")]
        public string DocumentNumber { get; set; }

        [JsonProperty("document_date")]
        public DateTime? DocumentDate { get; set; }

        [JsonProperty("document_place")]
        public string DocumentPlace { get; set; }

        [JsonProperty("company_activity")]
        public string CompanyActivity { get; set; }

        [JsonProperty("inn")]
        public string Inn { get; set; }

        [JsonProperty("address_reg")]
        public string AddressReg { get; set; }

        [JsonProperty("address_fact")]
        public string AddressFact { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("phone2")]
        public string Phone2 { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }
    }
}
