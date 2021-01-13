namespace DatingApi.Data.DataTransferObjects
{
    public class SearchUser
    {
        public int PageNumber { get; set; } = 1;
        
        private readonly int maxPageSize = 10;
        
        private int pageSize = 10;
        public int PageSize {
            get{
                return pageSize == 0 ? maxPageSize : pageSize;
            }
            set{
                pageSize = value >= 10 ? maxPageSize : value;
            }
        }

        private readonly int minValidAge = 1;

        private int minAge;
        public int MinAge
        {
            get {
                return minAge == 0 ? minValidAge : minAge;
            }
            set { 
                minAge = (value < minValidAge) ? minValidAge : value;
            }
        }

        private readonly int maxValidAge = 99;

        private int maxAge;
        public int MaxAge
        {
            get {
                return maxAge == 0 ? maxValidAge : maxAge;
            }
            set {
                maxAge = (value > maxValidAge) ? maxValidAge : value;
            }
        }

        private string gender;
        public string Gender
        {
            get {
                return gender ?? "all";
            }
            set { 
                    if(value.ToLowerInvariant() == "male")
                        gender = "male";
                    else if(value.ToLowerInvariant() == "female")
                        gender = "female";
                    else
                        gender = "all";
                }
        }
        
        private string orderBy;
        public string OrderBy
        {
            get {
                return string.IsNullOrWhiteSpace(orderBy) ? "created" : orderBy;
            }
            set { 
                    if(string.IsNullOrWhiteSpace(value))
                        orderBy = "created";
                    else
                        orderBy = value;
                }
        }
        
    }
}