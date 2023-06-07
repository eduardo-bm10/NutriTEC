namespace Postgre_API.Functions {
    public class ViewTotalRecipeCalories {
        public int? RecipeId { get; set; }
        public string RecipeDescription { get; set; }
        public string ProductDescription { get; set; }
        public Double? ProductCalories { get; set; }
        public int? ProductPortion { get; set; }
        public Double? TotalProductCalories { get; set; }
        public Double? TotalRecipeCalories { get; set; }
    }
}