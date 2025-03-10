namespace  lsclient.Server.Shared
{
     using System;
    using System.Security.Cryptography;
    using System.Text;
    using BCrypt.Net;
    using Microsoft.EntityFrameworkCore;

    public static class Functions
    {
        public static string GenerateCompanyId()
        {
            return GenerateId("CMP");
        }

        public static string GenerateContractId()
        {
            return GenerateId("CNT");

        }
        public static string GenerateCustomerId()
        {
            return GenerateId("CST");
        }

        public static string GenerateDriverId()
        {
            return GenerateId("DRV");
        }

        public static string GenerateTruckId()
        {
            return GenerateId("TRK");
        }

        public static string GenerateJobRequestId()
        {
            return GenerateId("JBR");
        }

        public static string GeneratePriceAgreementId()
        {
            return GenerateId("PAG");
        }
        public static string GeneratePaymentId()
        {
            return GenerateId("PYM");
        }

        public static string GenerateLocationId()
        {
            return GenerateId("LCN");
        }

        public static string GenerateId(string prefix)
        {
            Random random = new Random();
            int uniqueNumber = random.Next(10000000, 99999999); // Generate an 8-digit number
            return $"{prefix}{uniqueNumber}";
        }
      
            private static readonly string Key = "defaultPassword@123"; // Ensure this key is securely stored

            public static string HashPassword(string password)
            {
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Key)))
                {
                    var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return Convert.ToBase64String(hashBytes);
                }
            }

            public static bool VerifyPassword(string password, string hashedPassword)
            {
                var hash = HashPassword(password);
                return hashedPassword == hash;
            }
        

      
        //public static bool VerifyPassword(string password, string hashedPassword)
        //{
        //     return BCrypt.Verify(password, hashedPassword);
        //}

        public static  string GenerateTruckTypeId()
        {
            return GenerateId("TRT");

        }

        internal static object GenerateJwtToken(string userID, string role)
        {
            throw new NotImplementedException();
        }
    }

}
