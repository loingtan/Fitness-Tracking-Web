using FitnessTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.ViewModels
{
    public class BodyweightSummaryViewModel
    {
        public IEnumerable<BodyweightRecord> AllRecords { get; private set; }
        public IEnumerable<BodyweightRecord> CurrentWeekRecords { get; private set; }
        public IEnumerable<BodyweightRecord> CurrentMonthRecords { get; private set; }
        public BodyweightRecord MostRecentRecord { get; private set; }
        public BodyweightTarget Target { get; private set; }
        public double? CurrentWeekProgress { get; private set; }
        public double? CurrentWeekAverage { get; private set; }
        public double? CurrentMonthProgress { get; private set; }
        public double? CurrentMonthAverage { get; private set; }
        public double? AllTimeProgress { get; private set; }
        public double? AllTimeAverage { get; private set; }
        public double? DistanceToTarget { get; private set; }
        public double? DailyProgressNeeded { get; private set; }
        public double? WeeklyProgressNeeded { get; private set; }

        public BodyweightSummaryViewModel(IEnumerable<BodyweightRecord> allRecords, BodyweightTarget target)
        {
            // Initialize properties with default values if input data is null or empty
            AllRecords = allRecords ?? Enumerable.Empty<BodyweightRecord>();
            Target = target ?? new BodyweightTarget();
            MostRecentRecord = allRecords?.FirstOrDefault();

            if (allRecords != null && allRecords.Any())
            {
                CurrentMonthRecords = GetRecordsForPeriod(allRecords, 28);
                CurrentWeekRecords = GetRecordsForPeriod(CurrentMonthRecords, 7);

                CalculateProgressAndAverages();
                CalculateTargetProgress();
            }
            else
            {
                // Set default values for progress and averages
                CurrentWeekProgress = 0;
                CurrentWeekAverage = 0;
                CurrentMonthProgress = 0;
                CurrentMonthAverage = 0;
                AllTimeProgress = 0;
                AllTimeAverage = 0;
                DistanceToTarget = 0;
                DailyProgressNeeded = 0;
                WeeklyProgressNeeded = 0;
            }
        }

        private IEnumerable<BodyweightRecord> GetRecordsForPeriod(IEnumerable<BodyweightRecord> records, int days)
        {
            return records.Where(record => record.Date >= DateTime.Today.AddDays(-days)).ToList();
        }

        private void CalculateProgressAndAverages()
        {
            if (CurrentWeekRecords != null && CurrentWeekRecords.Any())
            {
                var firstWeekWeight = CurrentWeekRecords.First().Weight;
                var lastWeekWeight = CurrentWeekRecords.Last().Weight;
                CurrentWeekProgress = firstWeekWeight - lastWeekWeight;
                CurrentWeekAverage = CurrentWeekProgress / 7;
            }

            if (CurrentMonthRecords != null && CurrentMonthRecords.Any())
            {
                var firstMonthWeight = CurrentMonthRecords.First().Weight;
                var lastMonthWeight = CurrentMonthRecords.Last().Weight;
                CurrentMonthProgress = firstMonthWeight - lastMonthWeight;
                CurrentMonthAverage = CurrentMonthProgress / 28;
            }

            if (AllRecords != null && AllRecords.Any())
            {
                var firstAllTimeWeight = AllRecords.First().Weight;
                var lastAllTimeWeight = AllRecords.Last().Weight;
                AllTimeProgress = firstAllTimeWeight - lastAllTimeWeight;
                AllTimeAverage = AllTimeProgress / (float)(AllRecords.First().Date - AllRecords.Last().Date).TotalDays * 7;
            }
        }

        private void CalculateTargetProgress()
        {
            if (Target == null)
                return;

            DistanceToTarget = Target.TargetWeight - MostRecentRecord.Weight;
            var daysToTarget = (Target.TargetDate - DateTime.Today).TotalDays;
            if (daysToTarget > 0)
            {
                DailyProgressNeeded = (float)(DistanceToTarget / daysToTarget);
                WeeklyProgressNeeded = DailyProgressNeeded * 7;
            }
        }
    }
}
