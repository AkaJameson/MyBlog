namespace MyBlog.Web.Client.Components
{
    public partial class Calendar
    {
        private DateTime CurrentMonth = DateTime.Today;
        private List<CalendarDay> Days = new();

        protected override void OnInitialized()
        {
            GenerateCalendar();
        }

        private void GenerateCalendar()
        {
            Days.Clear();

            var firstDay = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            // 填充前月空白
            int startDay = (int)firstDay.DayOfWeek;
            for (int i = 0; i < startDay; i++)
            {
                var prevDay = firstDay.AddDays(-(startDay - i));
                Days.Add(new CalendarDay
                {
                    Day = prevDay.Day,
                    IsCurrentMonth = false
                });
            }

            // 当月日期
            for (int i = 1; i <= lastDay.Day; i++)
            {
                var date = new DateTime(CurrentMonth.Year, CurrentMonth.Month, i);
                Days.Add(new CalendarDay
                {
                    Day = i,
                    IsCurrentMonth = true,
                    IsToday = date.Date == DateTime.Today
                });
            }
        }

        private void PrevMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
            GenerateCalendar();
        }

        private void NextMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
            GenerateCalendar();
        }

        private class CalendarDay
        {
            public int Day { get; set; }
            public bool IsCurrentMonth { get; set; }
            public bool IsToday { get; set; }
        }
    }
}
