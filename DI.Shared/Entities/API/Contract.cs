using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class Contract
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("contract_type")]
        public string ContractType { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("number_original")]
        public string NumberOriginal { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        //[JsonProperty("drivers")]
        //public List<Driver> Drivers { get; set; }

        [JsonProperty("company")]
        public int Company { get; set; }

        [JsonProperty("car")]
        public int Car { get; set; }

        [JsonProperty("client")]
        public int Client { get; set; }

        [JsonProperty("contract_start")]
        public DateTime ContractStart { get; set; }

        [JsonProperty("contract_end")]
        public DateTime ContractEnd { get; set; }

        [JsonProperty("car_document_series")]
        public string CarDocumentSeries { get; set; }

        [JsonProperty("car_document_number")]
        public string CarDocumentNumber { get; set; }

        [JsonProperty("car_document_date")]
        public DateTime? CarDocumentDate { get; set; }

        [JsonProperty("contract_date")]
        public DateTime ContractDate { get; set; }

        [JsonProperty("product")]
        public string Product { get; set; }

        [JsonProperty("prev_number")]
        public string PrevNumber { get; set; }

        [JsonProperty("insurance_premium")]
        public string InsurancePremium { get; set; }

        [JsonProperty("premium_foreseen")]
        public string PremiumForeseen { get; set; }

        [JsonProperty("sum_insured")]
        public string SumInsured { get; set; }

        [JsonProperty("franchise")]
        public string Franchise { get; set; }

        [JsonProperty("kbm_current")]
        public string KbmCurrent { get; set; }

        [JsonProperty("kbm_foreseen")]
        public string KbmForeseen { get; set; }

        [JsonProperty("refund_option")]
        public string RefundOption { get; set; }

        [JsonProperty("discount")]
        public string Discount { get; set; }

        [JsonProperty("bonus")]
        public string Bonus { get; set; }

        [JsonProperty("quotation")]
        public int Quotation { get; set; }

        [JsonProperty("payed")]
        public int Payed { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("pay_date")]
        public DateTime? PayDate { get; set; }

        [JsonProperty("insurance_conditions")]
        public string InsuranceConditions { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("service_conditions")]
        public string ServiceConditions { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }
                      
        [JsonProperty("company_description")]
        public string CompanyDesrciption { get; set; }
    }
}
