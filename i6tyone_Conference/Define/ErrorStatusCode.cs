using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace eGhis_WebService_Core.Define
{
    public enum ErrorStatusCode
    {
        [Display(Name = "0001")]
        [Description("Information")]
        Info,

        [Display(Name = "0000")]
        [Description("Success")]
        Success,

        [Display(Name = "1001")]
        [Description("Invalid Error")]
        Invalid_Error,

        [Display(Name = "1002")]
        [Description("Decrypt Error")]
        Decrypt_Error,

        [Display(Name = "1003")]
        [Description("Encrypt Error")]
        Encrypt_Error,

        // ✅ DB 관련
        [Display(Name = "1004")]
        [Description("DB Decrypt Error")]
        DB_Decrypt_Error,

        [Display(Name = "1005")]
        [Description("DB Encrypt Error")]
        DB_Encrypt_Error,

        [Display(Name = "1006")]
        [Description("DB Insert Error")]
        DB_Insert_Error,

        [Display(Name = "1007")]
        [Description("DB Update Error")]
        DB_Update_Error,

        [Display(Name = "1008")]
        [Description("DB Delete Error")]
        DB_Delete_Error,

        [Display(Name = "1100")]
        [Description("Casting Error")]
        Casting_Error,

        [Display(Name = "2000")]
        [Description("Environment Null")]
        Environment_Null,

        [Display(Name = "2001")]
        [Description("DB Start Error")]
        DB_Start_Error,

        [Display(Name = "2002")]
        [Description("DB General Error")]
        DB_Error,

        [Display(Name = "2003")]
        [Description("DB Insert Invalid")]
        DB_Insert_Invalid,

        [Display(Name = "2004")]
        [Description("DB Update Invalid")]
        DB_Update_Invalid,

        [Display(Name = "2005")]
        [Description("DB Delete Invalid")]
        DB_Delete_Invalid,

        [Display(Name = "3001")]
        [Description("API Sync Update Error")]
        Api_Sync_Update_Error,

        [Display(Name = "4000")]
        [Description("Admin account not found")]
        Admin_Account_Not_Found,

        [Display(Name = "4001")]
        [Description("Authentication failed.")]
        Authentication_Failed,

        [Display(Name = "4002")]
        [Description("Token Invalid Error")]
        Token_Invalid_Error,

        [Display(Name = "4003")]
        [Description("Token Decrypt Error")]
        Token_Decrypt_Error,

        [Display(Name = "4004")]
        [Description("2FA authentication is required.")]
        Require_2FA_Authentication,

        [Display(Name = "4005")]
        [Description("Already 2FA registered")]
        Already_2FA_Registered,

        [Display(Name = "4006")]
        [Description("Otp verify fail")]
        Otp_Verify_Fail,

        [Display(Name = "4007")]
        [Description("Admin account inactive")]
        Admin_Account_Inactive,

        [Display(Name = "4008")]
        [Description("Auth temp token invalid")]
        Auth_Temp_Token_Invalid,

        [Display(Name = "4009")]
        [Description("Auth temp token expired")]
        Auth_Temp_Token_Expired,

        [Display(Name = "4010")]
        [Description("Password change failed")]
        Password_Change_Failed,

      

        [Display(Name = "6001")]
        [Description("File upload failed")]
        File_Upload_Fail,



        [Display(Name = "9999")]
        [Description("Unknown Error")]
        Error
    }
}
