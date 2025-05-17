/// <reference path="Base/XPopupElement.ts" />
/// <reference path="Base/XDropDownElement.ts" />
class XCalendar extends XDropDownElement
{
    constructor(pOwner: XElement, pClass: string)
    {
        super(pOwner, pClass);
        this.Header = new XDiv(this.HTML, "XCalendar-Header");
        this.LeftArrow = new XBaseButton(this.Header, "XCalendarLeftArrow");
        this.CenterButton = new XBaseButton(this.Header, "XCalendarCenterButton");
        this.RightArrow = new XBaseButton(this.Header, "XCalendarRightArrow");

        this.DaysGrid = new XDiv(this, "XDaysGrid");
        this.DaysGrid.UseVisibility = true;

        this.MonthsGrid = new XDiv(this, "XMonthsGrid");
        this.MonthsGrid.UseVisibility = true;
        this.MonthsGrid.IsVisible = false;

        this.YearsGrid = new XDiv(this, "XYearsGrid");
        this.YearsGrid.UseVisibility = true;
        this.YearsGrid.IsVisible = false;

        this.ViewDate = new Date();
        this.SelectedDate = new Date();
        this.UpdateCalendar();
        this.CenterButton.HTML.addEventListener('click', () =>
        {
            this.CurrentPanel = this.CurrentPanel === 'days' ? 'months' : 'years';
            this.UpdateCalendar();
        });
        this.LeftArrow.HTML.addEventListener('click', () => this.Navigate(-1));
        this.RightArrow.HTML.addEventListener('click', () => this.Navigate(1));
    }

    protected Header: XDiv;
    protected LeftArrow: XBaseButton;
    protected CenterButton: XBaseButton;
    protected RightArrow: XBaseButton;
    protected DaysGrid: XDiv;
    protected MonthsGrid: XDiv;
    protected YearsGrid: XDiv;
    private CurrentPanel: 'days' | 'months' | 'years' = 'days';
    private ViewDate: Date;
    SelectedDate: Date;
    OnSelectdate: XMethod<Date> | null = null;
    override get CanDrag(): boolean { return false; }
    override get CanResize(): boolean { return false; }

    override OnShow(pValue: boolean = true)
    {
        this.CurrentPanel = 'days';
        this.UpdateCalendar();
    }

    override OnHide()
    {
        if (this.DaysGrid == null)
            return;
        this.DaysGrid.IsVisible = false;
        this.MonthsGrid.IsVisible = false;
        this.YearsGrid.IsVisible = false;
    }

    override CallPopupClosed(): void
    {
        this.DaysGrid.IsVisible = false;
        this.MonthsGrid.IsVisible = false;
        this.YearsGrid.IsVisible = false;
    }

    private ShowYears()
    {
        this.YearsGrid.IsVisible = true;
        this.YearsGrid.HTML.innerHTML = "";

        const currentYear = this.ViewDate.getFullYear();

        const decadeStart = currentYear - ((currentYear - 1) % 10) - 1;
        const decadeEnd = decadeStart + 10;
        const gridStartYear = decadeStart - (decadeStart % 16);

        for (let year = gridStartYear; year < gridStartYear + 16; year++)
        {
            const cell = document.createElement('div');
            cell.className = 'YearCell';
            cell.textContent = year.toString();

            const isCurrentDecade = year >= (decadeStart + 1) && year <= decadeEnd;

            if (!isCurrentDecade)
                cell.classList.add('Faded');
            if (year === new Date().getFullYear())
                cell.classList.add('Current');

            cell.addEventListener('click', () =>
            {
                this.ViewDate.setFullYear(year);
                this.CurrentPanel = 'months';
                this.UpdateCalendar();
            });

            this.YearsGrid.HTML.appendChild(cell);
        }
    }

    private ShowMonths() 
    {
        this.MonthsGrid.IsVisible = true;
        this.MonthsGrid.HTML.innerHTML = "";
        for (let month = 0; month < 12; month++)
        {
            const cell = document.createElement('div');
            cell.className = 'MonthCell';
            cell.textContent = new Date(this.ViewDate.getFullYear(), month).toLocaleDateString('pt-BR', { month: 'long' });

            if (month === new Date().getMonth() && this.ViewDate.getFullYear() === new Date().getFullYear())
                cell.classList.add('Current');

            cell.addEventListener('click', () =>
            {
                this.ViewDate.setMonth(month);
                this.CurrentPanel = 'days';
                this.UpdateCalendar();
            });

            this.MonthsGrid.HTML.appendChild(cell);
        }
    }

    private ShowDays()
    {
        this.DaysGrid.IsVisible = true;
        this.DaysGrid.HTML.innerHTML = '';
        ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'].forEach((day, i) =>
        {
            const cell = document.createElement('div');
            cell.textContent = day;
            cell.className = `Day-Header ${i === 0 ? 'Sunday' : ''} ${i === 6 ? 'Saturday' : ''}`;
            this.DaysGrid.HTML.appendChild(cell);
        });

        const firstDay = new Date(this.ViewDate.getFullYear(), this.ViewDate.getMonth(), 1);
        const lastDay = new Date(this.ViewDate.getFullYear(), this.ViewDate.getMonth() + 1, 0);

        let date = new Date(firstDay);
        date.setDate(date.getDate() - firstDay.getDay());

        for (let i = 0; i < 42; i++)
        {
            const cell = document.createElement('div');
            cell.className = 'DayCell';

            const isCurrentMonth = date.getMonth() === this.ViewDate.getMonth();
            const isToday = date.toDateString() === new Date().toDateString();
            const isSelected = this.SelectedDate && date.toDateString() === this.SelectedDate.toDateString();

            if (!isCurrentMonth)
                cell.classList.add('Faded');
            if (isToday)
                cell.classList.add('Current');
            if (isSelected)
                cell.classList.add('Selected');

            cell.textContent = date.getDate().toString();
            let daydate = new Date(date);
            cell.addEventListener('click', () => this.SelectDate(daydate));

            if (date.getDay() === 0)
                cell.classList.add('Sunday');
            if (date.getDay() === 6)
                cell.classList.add('Saturday');

            this.DaysGrid.HTML.appendChild(cell);
            date.setDate(date.getDate() + 1);
        }
    }

    SelectDate(pDate: Date)
    {
        this.SelectedDate = pDate;
        this.ViewDate = new Date(pDate);
        this.UpdateCalendar();
        if (this.OnSelectdate != null)
            this.OnSelectdate.apply(this, [pDate]);
    }

    private Navigate(pDirection: number)
    {
        switch (this.CurrentPanel)
        {
            case 'days':
                this.ViewDate.setMonth(this.ViewDate.getMonth() + pDirection);
                break;

            case 'months':
                this.ViewDate.setFullYear(this.ViewDate.getFullYear() + pDirection);
                break;

            default:
                this.ViewDate.setFullYear(this.ViewDate.getFullYear() + (pDirection * 16));
                break;
        }
        this.UpdateCalendar();
    }

    UpdateCalendar()
    {
        this.DaysGrid.IsVisible = false;
        this.MonthsGrid.IsVisible = false;
        this.YearsGrid.IsVisible = false;
        switch (this.CurrentPanel)
        {
            case 'days':
                this.ShowDays();
                this.CenterButton.SetContent(this.ViewDate.toLocaleDateString('pt-BR', { month: 'long', year: 'numeric' }));
                break;

            case 'months':
                this.ShowMonths();
                this.CenterButton.SetContent(this.ViewDate.getFullYear().toString());
                break;

            default:
                this.ShowYears();
                const year = this.ViewDate.getFullYear() - (this.ViewDate.getFullYear() % 16);
                this.CenterButton.SetContent(`${year} - ${year + 15}`);
                break;
        }
    }

    protected override CreateContainer(): HTMLElement
    {
        return XUtils.AddElement<HTMLElement>(null, "div", null);
    }
}