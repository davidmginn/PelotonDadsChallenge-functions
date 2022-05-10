using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace PelotonDadsChallenge.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Workout
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        [JsonProperty("workout_name")]
        public string WorkoutName { get; set; }
    }

    public class WorkoutCounts
    {
        [JsonProperty("total_workouts")]
        public int TotalWorkouts { get; set; }

        [JsonProperty("workouts")]
        public List<Workout> Workouts { get; set; }
    }

    public class Record : TableEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("raw_value")]
        public double RawValue { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("unit_slug")]
        public string UnitSlug { get; set; }

        [JsonProperty("workout_id")]
        public string WorkoutId { get; set; }

        [JsonProperty("workout_date")]
        public DateTime WorkoutDate { get; set; }
    }

    public class PersonalRecord
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("first_workout_date")]
        public DateTime FirstWorkoutDate { get; set; }

        [JsonProperty("reset_date")]
        public object ResetDate { get; set; }

        [JsonProperty("records")]
        public List<Record> Records { get; set; }
    }

    public class Streaks
    {
        [JsonProperty("current_weekly")]
        public int CurrentWeekly { get; set; }

        [JsonProperty("best_weekly")]
        public int BestWeekly { get; set; }

        [JsonProperty("start_date_of_current_weekly")]
        public int StartDateOfCurrentWeekly { get; set; }
    }

    public class Achievement
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class PelotonOverviewResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("workout_counts")]
        public WorkoutCounts WorkoutCounts { get; set; }

        [JsonProperty("personal_records")]
        public List<PersonalRecord> PersonalRecords { get; set; }

        [JsonProperty("streaks")]
        public Streaks Streaks { get; set; }

        [JsonProperty("achievements")]
        public List<Achievement> Achievements { get; set; }
    }


}
