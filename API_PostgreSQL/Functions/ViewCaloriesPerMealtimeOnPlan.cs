namespace Postgre_API.Functions {
    public class ViewCaloriesPerMealtimeOnPlan {
        public int? PlanId { get; set; }
        public string Mealtime { get; set; }
        public Double? TotalCalories { get; set; }
    }
}