using GanitShop.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace GanitShop.Infrastructure
{
    public class ProductDbManager : IProductDbManager
    {
        readonly string _connString;
        public ProductDbManager(DbProperties dbProperties)
        {

            _connString = dbProperties.ConnString;
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
        {
            ProductDto productDto = productDto = CreateProductDto(createProductDto);

            using (SqlConnection sqlConnection = new SqlConnection(_connString))
            {
                await sqlConnection.OpenAsync(); 

                using (SqlCommand sqlCommand = new SqlCommand("CreateProduct", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@Name", createProductDto.Name));
                    sqlCommand.Parameters.Add(new SqlParameter("@Code", createProductDto.Code));
                    sqlCommand.Parameters.Add(new SqlParameter("@Description", createProductDto.Description ?? ""));
                    sqlCommand.Parameters.Add(new SqlParameter("@ImageFilePathName", createProductDto.UniqueImageFileName));

                    SqlDataReader rdr = sqlCommand.ExecuteReader();

                    while (rdr.Read())
                    {
                        productDto.Id = Convert.ToInt32(rdr["Id"]);
                        productDto.CreationTime = Convert.ToDateTime(rdr["CreationTime"].ToString());
                    }
                }
            }

            return productDto;

            ProductDto CreateProductDto(CreateProductDto dto)
            {
                return new ProductDto()
                {
                    Name = dto.Name,

                    Code = dto.Code,

                    Description = dto.Description ?? "",

                    ImagePath = dto?.ProductImage?.FileName ?? "",

                };
            }
        }

        public async Task<List<ProductDto>> GetAllAsync(int offset, string name, string codeName, int count = 50)
        {
            var products = new List<ProductDto>();

            string sqlStringCommand = CreateSelectCommand();

            using (SqlConnection sqlConnection = new SqlConnection(_connString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sqlStringCommand, sqlConnection))
                {
                    if (!String.IsNullOrEmpty(name))
                        sqlCommand.Parameters.Add(new SqlParameter("@ProductName", name));

                    if (!String.IsNullOrEmpty(codeName))
                        sqlCommand.Parameters.Add(new SqlParameter("@CodeName", codeName));

                    SqlDataReader rdr = sqlCommand.ExecuteReader();

                    while (rdr.Read())
                    {
                        var product = SQLProductToProductDto(rdr);

                        products.Add(product);
                    }
                }
            }

            return products;

            string CreateSelectCommand()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT * FROM Products ");

                bool isWhereClauseUsed = false;

                if (!String.IsNullOrEmpty(name))
                {
                    sb.Append("WHERE Name LIKE CONCAT('%',@ProductName,'%') ");

                    isWhereClauseUsed = true;
                }

                if (!String.IsNullOrEmpty(codeName))
                {
                    if (!isWhereClauseUsed)
                        sb.Append("WHERE Code LIKE CONCAT('%',@CodeName,'%') ");
                    else
                        sb.Append("AND Code LIKE CONCAT('%',@CodeName,'%') ");
                }

                sb.Append("ORDER BY Id DESC ");

                sb.Append("OFFSET " + offset + " ROWS ");

                sb.Append("FETCH NEXT " + count + " ROWS ONLY; ");

                return sb.ToString();
            }
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            string sqlStringCommand = "SELECT * FROM Products WHERE Id=@Id";

            ProductDto product = null;

            using (SqlConnection sqlConnection = new SqlConnection(_connString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sqlStringCommand, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@Id", id));

                    SqlDataReader rdr = sqlCommand.ExecuteReader();

                    while (rdr.Read())
                    {
                        product = SQLProductToProductDto(rdr);
                    }
                }
            }
            return product;
        }

        public async Task UpdateAsync(int id, UpdateProductDto updateProductDto)
        {
            string imageName = updateProductDto?.ProductImage?.FileName ?? String.Empty;

            string sqlStringCommand = BuildSQLCommand();

            using (SqlConnection sqlConnection = new SqlConnection(_connString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sqlStringCommand, sqlConnection))
                {
                    if (!String.IsNullOrEmpty(updateProductDto.Name))
                        sqlCommand.Parameters.Add(new SqlParameter("@Name", updateProductDto.Name));

                    if (!String.IsNullOrEmpty(updateProductDto.Code))
                        sqlCommand.Parameters.Add(new SqlParameter("@Code", updateProductDto.Code));

                    if (!String.IsNullOrEmpty(updateProductDto.Description))
                        sqlCommand.Parameters.Add(new SqlParameter("@Description", updateProductDto.Description));

                    if (!String.IsNullOrEmpty(updateProductDto.UniqueImageFileName))
                        sqlCommand.Parameters.Add(new SqlParameter("@ImageFilePathName", updateProductDto.UniqueImageFileName));

                    sqlCommand.Parameters.Add(new SqlParameter("@Id", id));

                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }

            string BuildSQLCommand()
            {
                var sBuilder = new StringBuilder();

                sBuilder.Append("UPDATE Products SET ");

                bool firstParameterUsed = false;

                if (!String.IsNullOrEmpty(updateProductDto.Name))
                {
                    sBuilder.Append("Name= @Name ");
                    firstParameterUsed = true;
                }
                   

                if (!String.IsNullOrEmpty(updateProductDto.Code))
                {
                    if (firstParameterUsed) 
                        sBuilder.Append(", "); 

                    sBuilder.Append("Code= @Code ");

                    firstParameterUsed = true;
                } 

                if (!String.IsNullOrEmpty(updateProductDto.Description))
                {
                    if (firstParameterUsed)
                        sBuilder.Append(", ");

                    sBuilder.Append("Description= @Description ");

                    firstParameterUsed = true;
                } 

                if (!String.IsNullOrEmpty(updateProductDto.UniqueImageFileName))
                {
                    if (firstParameterUsed)
                        sBuilder.Append(", ");

                    sBuilder.Append("ImageFilePathName= @ImageFilePathName ");
                }
                 
                sBuilder.Append("WHERE Id= @Id ");

                return sBuilder.ToString();
            }

        }

        public async Task DeleteByIdAsync(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM Products WHERE Id=@Id", sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@Id", id));

                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
        }

        private ProductDto SQLProductToProductDto(SqlDataReader rdr)
        {
            var product = new ProductDto();

            product.Id = Convert.ToInt32(rdr["Id"]);

            product.Name = rdr["Name"].ToString();

            product.Code = rdr["Code"].ToString();

            product.Description = rdr["Description"] != DBNull.Value ? rdr["Description"].ToString() : String.Empty;

            product.CreationTime = Convert.ToDateTime(rdr["CreationTime"]);

            product.ImagePath = rdr["ImageFilePathName"] != DBNull.Value ? rdr["ImageFilePathName"].ToString() : String.Empty;

            return product;
        }
    }

}
