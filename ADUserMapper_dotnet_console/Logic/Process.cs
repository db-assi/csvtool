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


            string[] columns = {"AccountExpirationDate","AccountLockoutTime","AccountNotDelegated","adminCount","AllowReversiblePasswordEncryption","BadLogonCount","c","CannotChangePassword","Certificates","City","CN","co","codePage","Company","Country","countryCode","Created","createTimeStamp","Deleted","Description","DistinguishedName","DoesNotRequirePreAuth","dSCorePropagationData","EmployeeID","EmployeeNumber","Enabled","Fax","fen-UM-AllowChargecodeOverride","fen-UM-AllowCoversheetOverride","fen-UM-AllowedToSendFax","fen-UM-AllowedToSendFaxInt","fen-UM-AllowedToSendSms","fen-UM-AllowedToSendSmsInt","fen-UM-AllowOurCSIDOverride","fen-UM-AllowSMSTemplateOverride","fen-UM-Coversheet","fen-UM-Fileformat","GivenName","HomeDirectory","HomedirRequired","HomeDrive","homeMDB","homeMTA","HomePage","HomePhone","Initials","instanceType","isDeleted","l","LastBadPasswordAttempt","LastKnownParent","lastLogon","LastLogonDate","lastLogonTimestamp","legacyExchangeDN","LockedOut","logonCount","LogonWorkstations","mailNickname","managedObjects","Manager","mDBUseDefaults","MemberOf","MNSLogonAccount","MobilePhone","Modified","modifyTimeStamp","msDS-SupportedEncryptionTypes","msDS-User-Account-Control-Computed","msExchALObjectVersion","msExchArchiveDatabaseLink","msExchArchiveGUID","msExchArchiveName","msExchArchiveQuota","msExchArchiveWarnQuota","msExchCoManagedObjectsBL","msExchDelegateListBL","msExchELCMailboxFlags","msExchHomeServerName","msExchMailboxGuid","msExchMailboxSecurityDescriptor","msExchMailboxTemplateLink","msExchPoliciesIncluded","msExchRBACPolicyLink","msExchRecipientDisplayType","msExchRecipientTypeDetails","msExchShadowProxyAddresses","msExchTextMessagingState","msExchThrottlingPolicyDN","msExchUMDtmfMap","msExchUserAccountControl","msExchUserCulture","msExchVersion","msExchWhenMailboxCreated","mSMQDigests","mSMQSignCertificates","msTSExpireDate","msTSLicenseVersion","msTSManagingLS","Name","nTSecurityDescriptor","ObjectCategory","ObjectClass","ObjectGUID","objectSid","Organization","OtherName","PasswordExpired","PasswordLastSet","PasswordNeverExpires","PasswordNotRequired","POBox","PostalCode","PrimaryGroup","primaryGroupID","ProfilePath","ProtectedFromAccidentalDeletion","protocolSettings","proxyAddresses","pwdLastSet","SamAccountName","sAMAccountType","ScriptPath","sDRightsEffective","ServicePrincipalNames","showInAddressBook","SID","SIDHistory","SmartcardLogonRequired","sn","st","State","StreetAddress","Surname","textEncodedORAddress","mail","TrustedForDelegation","TrustedToAuthForDelegation","UseDESKeyOnly","userAccountControl","userCertificate","UserPrincipalName","uSNChanged","uSNCreated","whenChanged","whenCreated", "Division"};

            DtOperations.RemoveColumns(dt, columns);

            dt = DtOperations.Contains(dt, "CanonicalName", "NormalUsers");

            dt = DtOperations.IsNull(dt, "EmailAddress");

            string[] oldNames = { "DisplayName", "Title", "EmailAddress", "OfficePhone" };
            string[] newNames = {"Name", "Job Title", "Email", "Number" };

            dt = DtOperations.ChangeColumnName(dt, oldNames, newNames);

            string[] newCols = { "Floor", "Building" };

            dt = DtOperations.AddColumnDummyData(dt, newCols);

            dt = DtOperations.AddColumnsDefaultValue(dt, "Site", "GOSH");

            CsvOperations.DataTableToCsv(dt, "C:\\Users\\Bassid\\Documents\\Dev\\csvtool\\static\\output.csv");




        }
    }
}
