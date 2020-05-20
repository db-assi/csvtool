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

           DtOperations.RemoveColumns(dt, columns);

            var queries = new List<Dictionary<string, object>>();

            var q1 = new Dictionary<string, object>
            {
                ["Field"] = "CanonicalName",
                ["Operation"] = "Contain",
                ["Value"] = "NormalUsers",
                ["KeepNull"] = false
            };

            var q2 = new Dictionary<string, object>
            {
                ["Field"] = "CanonicalName",
                ["Operation"] = "Contain",
                ["Value"] = "ICTUsers",
                ["KeepNull"] = false
            };

            var q3 = new Dictionary<string, object>
            {
                ["Field"] = "CanonicalName",
                ["Operation"] = "NotContain",
                ["Value"] = "GOSH-TEST",
                ["KeepNull"] = false
            };

            var q4 = new Dictionary<string, object>
            {
                ["Field"] = "EmailAddress",
                ["Operation"] = "IsNotNull",
                ["Value"] = 0,
                ["KeepNull"] = false
            };

            var queries1 = new List<Dictionary<string, object>>();

            //should start a new query to remove rows
            var q5 = new Dictionary<string, object>
            {
                ["Field"] = "accountExpires",
                ["Operation"] = "GreaterThan",
                ["Value"] = DateTime.Today.ToFileTime(),
                ["KeepNull"] = false
            };

            var q6 = new Dictionary<string, object>
            {
                ["Field"] = "accountExpires",
                ["Operation"] = "Equal",
                ["Value"] = long.Parse("0"),
                ["KeepNull"] = false
            };

            List<string> criteria = new List<string>();

            List<string> criteria1 = new List<string>();

            queries.Add(q1);
                criteria.Add("or");
            queries.Add(q2);
                criteria.Add("and");
            queries.Add(q3);
                criteria.Add("and");
            queries.Add(q4);
                

            queries1.Add(q5);
                criteria1.Add("or");
            queries1.Add(q6);

            dt = DtOperations.RemoveRows(dt, queries, criteria);

            dt = DtOperations.RemoveRows(dt, queries1, criteria1);

            string[] remove = { "accountExpires", "CanonicalName", "msDS-User-Account-Control-Computed" };

            dt = DtOperations.RemoveColumns(dt, remove);

            CsvOperations.DataTableToCsv(dt, "C:\\Users\\daian\\Documents\\c.projects\\DRIVE\\GSTT\\output\\preoutput.csv");

            DataRow[] rows = dt.Select();
            int lookUpColIndex = dt.Columns.IndexOf("OfficePhone");

            for (int i = 0; dt.Rows.Count > i; i++)
            {
                if (rows[i][lookUpColIndex].ToString().ToLower().Contains("extn"))
                {
                    string substring = StringOperations.RemoveAfter(rows[i][lookUpColIndex].ToString().ToLower(), "extn");
                    dt.Rows[i].SetField(lookUpColIndex, substring);
                }
                if (rows[i][lookUpColIndex].ToString().ToLower().Contains("ext"))
                {
                    string substring = StringOperations.RemoveAfter(rows[i][lookUpColIndex].ToString().ToLower(), "ext");
                    dt.Rows[i].SetField(lookUpColIndex, substring);
                }
                if (rows[i][lookUpColIndex].ToString().ToLower().Contains("ex"))
                {
                    string substring = StringOperations.RemoveAfter(rows[i][lookUpColIndex].ToString().ToLower(), "ex");
                    dt.Rows[i].SetField(lookUpColIndex, substring);
                }
                if (rows[i][lookUpColIndex].ToString().ToLower().Contains("x"))
                {
                    string substring = StringOperations.RemoveAfter(rows[i][lookUpColIndex].ToString().ToLower(), "x");
                    dt.Rows[i].SetField(lookUpColIndex, substring);
                }

            }

            for (int i = 0; dt.Rows.Count > i; i++)
            {
                if (rows[i][lookUpColIndex].ToString().ToLower().Contains("blp"))
                {
                    string substring = StringOperations.RemoveFrom(rows[i][lookUpColIndex].ToString().ToLower(), "blp");
                    dt.Rows[i].SetField(lookUpColIndex, substring);
                }
                if (rows[i][lookUpColIndex].ToString().ToLower().Contains("bleep"))
                {
                    string substring = StringOperations.RemoveFrom(rows[i][lookUpColIndex].ToString().ToLower(), "bleep");
                    dt.Rows[i].SetField(lookUpColIndex, substring);
                }
            }

            for (int i = 0; dt.Rows.Count > i; i++)
            {
                if (rows[i][lookUpColIndex].ToString().ToLower().Contains(":"))
                {
                    string substring = StringOperations.RemoveAfter(rows[i][lookUpColIndex].ToString().ToLower(), ":");
                    dt.Rows[i].SetField(lookUpColIndex, substring);
                }
                if (rows[i][lookUpColIndex].ToString().ToLower().Contains("."))
                {
                    string substring = StringOperations.RemoveAfter(rows[i][lookUpColIndex].ToString().ToLower(), ".");
                    dt.Rows[i].SetField(lookUpColIndex, substring);
                }
            }

            for (int i = 0; dt.Rows.Count > i; i++)
            {
                if (rows[i][lookUpColIndex].ToString().Length == 2)
                {

                    dt.Rows[i].SetField(lookUpColIndex, "00" + rows[i][lookUpColIndex].ToString());

                }

                if (rows[i][lookUpColIndex].ToString().Length == 3)
                {

                    dt.Rows[i].SetField(lookUpColIndex, "0" + rows[i][lookUpColIndex].ToString());

                }
            }

            for (int i = 0; dt.Rows.Count > i; i++)
            {
                if (rows[i][lookUpColIndex].ToString().ToLower().Contains("appl"))
                {
                    dt.Rows[i].SetField(lookUpColIndex, "");
                }
            }


                CsvOperations.DataTableToCsv(dt, "C:\\Users\\daian\\Documents\\c.projects\\DRIVE\\GSTT\\output\\output.csv");




        }
    }
}
