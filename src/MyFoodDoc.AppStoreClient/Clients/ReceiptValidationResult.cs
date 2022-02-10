using System;

namespace MyFoodDoc.AppStoreClient.Clients
{
    public class ReceiptValidationResult
    {
        public DateTime? PurchaseDate { get; set; }

        public DateTime? SubscriptionExpirationDate { get; set; }

        public string ProductId { get; set; }

        public string OriginalTransactionId { get; set; }
    }

    public class Rootobject
    {
        public string environment { get; set; }
        public Receipt receipt { get; set; }
        public Latest_Receipt_Info[] latest_receipt_info { get; set; }
        public string latest_receipt { get; set; }
        public Pending_Renewal_Info[] pending_renewal_info { get; set; }
        public int status { get; set; }
    }

    public class Receipt
    {
        public string receipt_type { get; set; }
        public int adam_id { get; set; }
        public int app_item_id { get; set; }
        public string bundle_id { get; set; }
        public string application_version { get; set; }
        public long download_id { get; set; }
        public int version_external_identifier { get; set; }
        public string receipt_creation_date { get; set; }
        public string receipt_creation_date_ms { get; set; }
        public string receipt_creation_date_pst { get; set; }
        public string request_date { get; set; }
        public string request_date_ms { get; set; }
        public string request_date_pst { get; set; }
        public string original_purchase_date { get; set; }
        public string original_purchase_date_ms { get; set; }
        public string original_purchase_date_pst { get; set; }
        public string original_application_version { get; set; }
        public In_App[] in_app { get; set; }
    }

    public class In_App
    {
        public string quantity { get; set; }
        public string product_id { get; set; }
        public string transaction_id { get; set; }
        public string original_transaction_id { get; set; }
        public string purchase_date { get; set; }
        public string purchase_date_ms { get; set; }
        public string purchase_date_pst { get; set; }
        public string original_purchase_date { get; set; }
        public string original_purchase_date_ms { get; set; }
        public string original_purchase_date_pst { get; set; }
        public string expires_date { get; set; }
        public string expires_date_ms { get; set; }
        public string expires_date_pst { get; set; }
        public string web_order_line_item_id { get; set; }
        public string is_trial_period { get; set; }
        public string is_in_intro_offer_period { get; set; }
    }

    public class Latest_Receipt_Info
    {
        public string quantity { get; set; }
        public string product_id { get; set; }
        public string transaction_id { get; set; }
        public string original_transaction_id { get; set; }
        public string purchase_date { get; set; }
        public string purchase_date_ms { get; set; }
        public string purchase_date_pst { get; set; }
        public string original_purchase_date { get; set; }
        public string original_purchase_date_ms { get; set; }
        public string original_purchase_date_pst { get; set; }
        public string expires_date { get; set; }
        public string expires_date_ms { get; set; }
        public string expires_date_pst { get; set; }
        public string web_order_line_item_id { get; set; }
        public string is_trial_period { get; set; }
        public string is_in_intro_offer_period { get; set; }
        public string subscription_group_identifier { get; set; }
    }

    public class Pending_Renewal_Info
    {
        public string expiration_intent { get; set; }
        public string auto_renew_product_id { get; set; }
        public string original_transaction_id { get; set; }
        public string is_in_billing_retry_period { get; set; }
        public string product_id { get; set; }
        public string auto_renew_status { get; set; }
    }

}
