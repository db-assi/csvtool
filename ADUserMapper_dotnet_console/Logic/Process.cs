using ADUserMapper_dotnet_console.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADUserMapper_dotnet_console.Logic
{
    public static class Process
    {
        public static void processAD()
        {
            string path = FileOperations.OpenFileDialog();

            DataTable dt = CsvOperations.CsvToDataTable(path);

            #region Columns
            string[] columns = {"AccountExpirationDate","AccountLockoutTime","AccountNotDelegated","adminCount","AllowReversiblePasswordEncryption","BadLogonCount","c","CannotChangePassword","Certificates","City","CN","co","codePage","Company","Country","countryCode","Created","createTimeStamp","Deleted","Description","DistinguishedName","DoesNotRequirePreAuth","dSCorePropagationData","EmployeeID","EmployeeNumber","Enabled","Fax","fen-UM-AllowChargecodeOverride","fen-UM-AllowCoversheetOverride","fen-UM-AllowedToSendFax","fen-UM-AllowedToSendFaxInt","fen-UM-AllowedToSendSms","fen-UM-AllowedToSendSmsInt","fen-UM-AllowOurCSIDOverride","fen-UM-AllowSMSTemplateOverride","fen-UM-Coversheet","fen-UM-Fileformat","GivenName","HomeDirectory","HomedirRequired","HomeDrive","homeMDB","homeMTA","HomePage","HomePhone","Initials","instanceType","isDeleted","l","LastBadPasswordAttempt","LastKnownParent","lastLogon","LastLogonDate","lastLogonTimestamp","legacyExchangeDN","LockedOut","logonCount","LogonWorkstations","mailNickname","managedObjects","Manager","mDBUseDefaults","MemberOf","MNSLogonAccount","MobilePhone","Modified","modifyTimeStamp","msDS-SupportedEncryptionTypes","msExchALObjectVersion","msExchArchiveDatabaseLink","msExchArchiveGUID","msExchArchiveName","msExchArchiveQuota","msExchArchiveWarnQuota","msExchCoManagedObjectsBL","msExchDelegateListBL","msExchELCMailboxFlags","msExchHomeServerName","msExchMailboxGuid","msExchMailboxSecurityDescriptor","msExchMailboxTemplateLink","msExchPoliciesIncluded","msExchRBACPolicyLink","msExchRecipientDisplayType","msExchRecipientTypeDetails","msExchShadowProxyAddresses","msExchTextMessagingState","msExchThrottlingPolicyDN","msExchUMDtmfMap","msExchUserAccountControl","msExchUserCulture","msExchVersion","msExchWhenMailboxCreated","mSMQDigests","mSMQSignCertificates","msTSExpireDate","msTSLicenseVersion","msTSManagingLS","Name","nTSecurityDescriptor","ObjectCategory","ObjectClass","ObjectGUID","objectSid","Organization","OtherName","PasswordExpired","PasswordLastSet","PasswordNeverExpires","PasswordNotRequired","POBox","PostalCode","PrimaryGroup","primaryGroupID","ProfilePath","ProtectedFromAccidentalDeletion","protocolSettings","proxyAddresses","pwdLastSet","SamAccountName","sAMAccountType","ScriptPath","sDRightsEffective","ServicePrincipalNames","showInAddressBook","SID","SIDHistory","SmartcardLogonRequired","sn","st","State","StreetAddress","Surname","textEncodedORAddress","mail","TrustedForDelegation","TrustedToAuthForDelegation","UseDESKeyOnly","userAccountControl","userCertificate","UserPrincipalName","uSNChanged","uSNCreated","whenChanged","whenCreated", "Division"};
            #endregion

           //DtOperations.RemoveColumns(dt, columns);

            var queries = new List<Dictionary<string, object>>();

            //var q1 = new Dictionary<string, object>
            //{
            //    ["Field"] = "CanonicalName",
            //    ["Method"] = "Contains",
            //    ["Value"] = "NormalUsers"
            //};

            //var q2 = new Dictionary<string, object>
            //{
            //    ["Field"] = "CanonicalName",
            //    ["Method"] = "Contains",
            //    ["Value"] = "ICTUsers"
            //};

            //var q3 = new Dictionary<string, object>
            //{
            //    ["Field"] = "CanonicalName",
            //    ["Method"] = "Contains",
            //    ["Unary"] = true,
            //    ["Value"] = "GOSH-TEST"
            //};

            //var q4 = new Dictionary<string, object>
            //{
            //    ["Field"] = "EmailAddress",
            //    ["Property"] = "Length",
            //    ["Comparisson"] = "NotEqual",
            //    ["Value"] = 0
            //};

            //var q5 = new Dictionary<string, object>
            //{
            //    ["Field"] = "accountExpires",
            //    ["Comparisson"] = "LessThan",
            //    ["Value"] = DateTime.Today.ToFileTime()
            //};

            var q6 = new Dictionary<string, object>
            {
                ["Field"] = "msDS-User-Account-Control-Computed",
                ["Property"] = "Length",
                ["Comparisson"] = "NotEqual",
                ["Value"] = 0
            };

            var q7 = new Dictionary<string, object>
            {
                ["Field"] = "msDS-User-Account-Control-Computed",
                ["Comparisson"] = "NotEqual",
                ["Value"] = long.Parse("0")
            };

            List<string> criteria = new List<string>();

            //queries.Add(q1);
            //    criteria.Add("or");
            //queries.Add(q2);
            //    criteria.Add("and");
            //queries.Add(q3);
            //    criteria.Add("and");
            //queries.Add(q4);
            //    criteria.Add("and");
            queries.Add(q7);
            //    criteria.Add("and");
            //queries.Add(q6);

            dt = DtOperations.RemoveRows(dt, queries, criteria);

            //string[] oldNames = { "DisplayName", "Title", "EmailAddress", "OfficePhone" };
            //string[] newNames = {"Name", "Job Title", "Email", "Number" };

           // dt = DtOperations.ChangeColumnName(dt, oldNames, newNames);

            CsvOperations.DataTableToCsv(dt, "C:\\Users\\daian\\Documents\\c.projects\\DRIVE\\GSTT\\" + DateTime.UtcNow.ToFileTime() + ".csv");




        }
    }
}
