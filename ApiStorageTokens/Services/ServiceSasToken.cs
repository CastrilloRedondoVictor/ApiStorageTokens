using Azure.Data.Tables;
using Azure.Data.Tables.Sas;

namespace ApiStorageTokens.Services
{
    public class ServiceSasToken
    {
        private TableClient _tableClient;

        public ServiceSasToken(IConfiguration configuration)
        {
            var azureKeys = configuration["AzureKeys:StorageAccount"];
            TableServiceClient tableServiceClient = new TableServiceClient(azureKeys);

            _tableClient = tableServiceClient.GetTableClient("alumnos");
        }

        public string GenerateToken(string curso)
        {
            TableSasPermissions permissions = TableSasPermissions.Read;

            TableSasBuilder sasBuilder = this._tableClient.GetSasBuilder(permissions, DateTime.UtcNow.AddMinutes(30));

            sasBuilder.PartitionKeyStart = curso;
            sasBuilder.PartitionKeyEnd = curso;

            Uri uriToken = this._tableClient.GenerateSasUri(sasBuilder);

            return uriToken.AbsoluteUri;
        }
    }
}
