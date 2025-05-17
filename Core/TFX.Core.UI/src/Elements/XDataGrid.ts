/// <reference path="XDiv.ts" />


class XDataGrid extends XDiv
{
        
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null)
    {
        super(pOwner, pClass);
        //var data = [];
        //for (let i = 0; i < 20; i++)
        //{
        //    const row: any = {
        //        id: i,
        //        nome: `Nome ${i}`,
        //        email: `email${i}@exemplo.com`,
        //        cidade: `Cidade ${i % 100}`,
        //        idade: 20 + (i % 50),
        //        telefone: `(11) 9${Math.floor(1000 + Math.random() * 9000)}-${Math.floor(1000 + Math.random() * 9000)}`,
        //        empresa: `Empresa ${i % 20}`,
        //        cargo: `Cargo ${i % 10}`,
        //        salario: 2000 + (i % 50) * 100,
        //        dataAdmissao: `${(i % 28 + 1).toString().LPad(2, '0')}/01/2023`,
        //        status: i % 4 === 0 ? 'Ativo' : 'Inativo',
        //        cargo1: `Cargo ${i % 10}`,
        //        salario2: 2000 + (i % 50) * 100,
        //        dataAdmissao3: `${(i % 28 + 1).toString().LPad(2, '0')}/01/2023`,
        //        nome1: `Nome ${i}`,
        //        email1: `email${i}@exemplo.com`,
        //        cidade1: `Cidade ${i % 100}`,
        //        idade1: 20 + (i % 50),
        //        telefone1: `(11) 9${Math.floor(1000 + Math.random() * 9000)}-${Math.floor(1000 + Math.random() * 9000)}`,
        //        empresa1: `Empresa ${i % 20}`,
        //    };
        //    data.push(row);
        //}
        this.Table = new XTable(this, "XTable");
        //this.Table.SetDataSet(data);
    }

    Table: XTable;
}



//class XDataGridx extends XElement
//{
//    data: any = [];
//    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null)
//    {
//        super(pOwner, pClass);

//        for (let i = 0; i < 100; i++)
//        {
//            const row: any = {
//                id: i,
//                nome: `Nome ${i}`,
//                email: `email${i}@exemplo.com`,
//                cidade: `Cidade ${i % 100}`,
//                idade: 20 + (i % 50),
//                telefone: `(11) 9${Math.floor(1000 + Math.random() * 9000)}-${Math.floor(1000 + Math.random() * 9000)}`,
//                empresa: `Empresa ${i % 20}`,
//                cargo: `Cargo ${i % 10}`,
//                salario: 2000 + (i % 50) * 100,
//                dataAdmissao: `${(i % 28 + 1).toString().LPad(2, '0')}/01/2023`,
//                status: i % 4 === 0 ? 'Ativo' : 'Inativo',
//                cargo1: `Cargo ${i % 10}`,
//                salario2: 2000 + (i % 50) * 100,
//                dataAdmissao3: `${(i % 28 + 1).toString().LPad(2, '0')}/01/2023`,
//                nome1: `Nome ${i}`,
//                email1: `email${i}@exemplo.com`,
//                cidade1: `Cidade ${i % 100}`,
//                idade1: 20 + (i % 50),
//                telefone1: `(11) 9${Math.floor(1000 + Math.random() * 9000)}-${Math.floor(1000 + Math.random() * 9000)}`,
//                empresa1: `Empresa ${i % 20}`,
//            };
//            this.DataSet.push(row);
//        }
//        this.Container = new XDiv(this, "XDataGrid");
//        this.container = this.Container.HTML;
//        const fields = Object.keys(this.DataSet[0] || {});
//        this.Columns = fields.map(field => ({ field, visible: true, width: 120 }));
//        this.render();
//        this.addColumnVisibilityToggle();
//    }
//    Container: XDiv;
//    Table: XTable | null = null;
//    private container: HTMLElement;
//    private DataSet: any[] = [];
//    private Columns: XColumnConfig[];
//    private _SortState: { field: string; direction: 'asc' | 'desc' } | null = null;
//    private rowNumberColumn: XColumnConfig = { Title: '#', Visible: true, Width: 50 };

//    protected override CreateContainer(): HTMLElement 
//    {
//        return XUtils.AddElement<HTMLElement>(null, "div", null);
//    }

//    private render()
//    {
//        this.container.innerHTML = '';
//        this.Table = new XTable(this.Container, "");
//        const table = this.Table;
//        //table.style.minWidth = `${this.columns.reduce((acc, col) => acc + (col.visible ? col.width : 0), this.rowNumberColumn.width)}px`;
//        this.buildHeader(table);
//        //this.buildBody(table);
//        //this.container.appendChild(table);
//    }

//    private buildHeader(table: XTable)
//    {
//        this.Columns.filter(c => c.Visible).forEach(colConfig =>
//        {
//            const th = this.createHeaderTh(colConfig);
//            headerRow.appendChild(th);
//        });

//    }

//    private createHeaderTh(colConfig: XColumnConfig): HTMLTableCellElement
//    {
//        const th = document.createElement('th');
//        th.textContent = colConfig.Title;
//        //th.style.width = `${colConfig.width}px`;
//        th.style.userSelect = 'none';
//        //if (colConfig.field !== '#')
//        th.draggable = colConfig.Title !== '#';

//        const sortIcon = document.createElement('span');
//        sortIcon.className = 'sort-icon';
//        if (this._SortState?.field === colConfig.Title)
//        {
//            sortIcon.textContent = this._SortState.direction === 'asc' ? ' ▲' : ' ▼';
//        }
//        th.appendChild(sortIcon);

//        // Drag para reordenar colunas
//        th.addEventListener('dragstart', (e) =>
//        {
//            e.dataTransfer?.setData('text/plain', colConfig.Title);
//            th.classList.add('dragging');
//        });

//        th.addEventListener('dragend', () =>
//        {
//            th.classList.remove('dragging');
//        });

//        th.addEventListener('dragover', (e) =>
//        {
//            e.preventDefault();
//            if (!th.classList.contains('drag-over'))
//            {
//                th.classList.add('drag-over');
//            }
//        });

//        th.addEventListener('dragleave', () =>
//        {
//            th.classList.remove('drag-over');
//        });

//        th.addEventListener('drop', (e) =>
//        {
//            e.preventDefault();
//            th.classList.remove('drag-over');

//            const draggedField = e.dataTransfer?.getData('text/plain');
//            const visibleColumns = this.getVisibleColumns();

//            const draggedIndex = visibleColumns.findIndex(c => c.Title === draggedField);
//            const targetIndex = visibleColumns.findIndex(c => c.Title === colConfig.Title);

//            if (draggedIndex === -1 || targetIndex === -1 || draggedIndex === targetIndex) return;

//            // Reordenar colunas originais mantendo a referência
//            const originalDraggedIndex = this.Columns.findIndex(c => c.Title === draggedField);
//            const originalTargetIndex = this.Columns.findIndex(c => c.Title === colConfig.Title);

//            [this.Columns[originalDraggedIndex], this.Columns[originalTargetIndex]] =
//                [this.Columns[originalTargetIndex], this.Columns[originalDraggedIndex]];

//            this.render();
//        });


//        // Redimensionador
//        const resizer = document.createElement('div');
//        resizer.className = 'resizer';
//        th.appendChild(resizer);
//        this.addResizerEvents(th, resizer, colConfig);

//        // Clique para ordenar
//        th.addEventListener('click', () => this.sortData(colConfig.Title));
//        return th;
//    }
//    private addResizerEvents(th: HTMLTableCellElement, resizer: HTMLDivElement, colConfig: XColumnConfig)
//    {
//        let isResizing = false;
//        let startX = 0;
//        let startWidth = 0;

//        resizer.addEventListener('mousedown', (e) =>
//        {
//            e.stopPropagation(); // Impede a propagação para o elemento pai
//            e.preventDefault(); // Evita comportamento padrão indesejado

//            isResizing = true;
//            startX = e.clientX;
//            startWidth = th.offsetWidth;

//            const handleMouseMove = (e: MouseEvent) =>
//            {
//                if (!isResizing) return;
//                const newWidth = startWidth + (e.clientX - startX);
//                th.style.width = `${newWidth}px`;
//                colConfig.Width = newWidth;
//                this.updateColumnWidths(colConfig.Title, newWidth);
//            };

//            const handleMouseUp = () =>
//            {
//                isResizing = false;
//                document.removeEventListener('mousemove', handleMouseMove);
//                document.removeEventListener('mouseup', handleMouseUp);
//            };

//            document.addEventListener('mousemove', handleMouseMove);
//            document.addEventListener('mouseup', handleMouseUp, { once: true });
//        });
//    }

//    private updateColumnWidths(field: string, width: number)
//    {
//        const index = this.Columns.findIndex(c => c.Title === field);
//        if (index > -1)
//        {
//            this.Columns[index].Width = width;
//            document.querySelectorAll(`th[data-field="${field}"], td[data-field="${field}"]`)
//                .forEach(el => (el as HTMLElement).style.width = `${width}px`);
//        }
//    }

//    private getVisibleColumns(): XColumnConfig[]
//    {
//        return [this.rowNumberColumn, ...this.Columns.filter(c => c.Visible)];
//    }


//    private sortData(field: string)
//    {
//        if (this._SortState?.field === field)
//        {
//            this._SortState.direction = this._SortState.direction === 'asc' ? 'desc' : 'asc';
//        } else
//        {
//            this._SortState = { field, direction: 'asc' };
//        }
//        var self = this._SortState;
//        this.DataSet.sort((a, b) =>
//        {
//            var e: any = this;
//            if (a[field] > b[field])
//                return e._SortState.direction === 'asc' ? 1 : -1;
//            if (a[field] < b[field])
//                return e._SortState.direction === 'asc' ? -1 : 1;
//            return 0;
//        });

//        this.render();
//    }

//    private addColumnVisibilityToggle()
//    {
//        const button = document.createElement('button');
//        button.className = 'menu-button';
//        button.textContent = '☰';

//        const menu = document.createElement('div');
//        menu.className = 'column-menu';

//        this.Columns.forEach(col =>
//        {
//            const label = document.createElement('label');
//            const checkbox = document.createElement('input');
//            checkbox.type = 'checkbox';
//            checkbox.checked = col.Visible;
//            checkbox.addEventListener('change', () =>
//            {
//                col.Visible = checkbox.checked;
//                this.render();
//            });
//            label.appendChild(checkbox);
//            label.appendChild(document.createTextNode(col.Title));
//            menu.appendChild(label);
//        });

//        button.addEventListener('click', () =>
//        {
//            menu.style.display = menu.style.display === 'block' ? 'none' : 'block';
//        });

//        this.HTML.appendChild(button);
//        this.HTML.appendChild(menu);
//    }

//    private buildBody(table: HTMLTableElement)
//    {
//        const tbody = document.createElement('tbody');
//        this.DataSet.forEach((rowData, index) =>
//        {
//            const tr = document.createElement('tr');

//            // Número sequencial
//            const tdNumber = document.createElement('td');
//            tdNumber.textContent = (index + 1).toString();
//            tr.appendChild(tdNumber);

//            // Dados
//            this.Columns.filter(c => c.Visible).forEach(colConfig =>
//            {
//                const td = document.createElement('td');
//                td.dataset.field = colConfig.Title;

//                td.style.width = `${colConfig.Width}px`;
//                tr.appendChild(td);
//                const txt = document.createElement('span');
//                txt.innerText = rowData[colConfig.Title];

//                td.appendChild(txt);

//            });

//            tbody.appendChild(tr);
//        });
//        table.appendChild(tbody);
//    }


//}



