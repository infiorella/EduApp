//Cantidad de acitvidades completadas or mes
var actividadesOptions = {
    series: [{
        name: 'Foro',
        data: [0, 0, 0,0,0,0,1,0,0,0]
    }, {
        name: 'Test',
        data: [0, 0, 0, 0, 0, 0, 1, 0, 0, 0]
    }, {
        name: 'Lectura',
        data: [0, 0, 0, 0, 0, 0, 1, 0, 0, 0]
    }],
    chart: {
        type: 'bar',
        height: 350
    },
    plotOptions: {
        bar: {
            horizontal: false,
            columnWidth: '55%',
            endingShape: 'rounded'
        },
    },
    dataLabels: {
        enabled: false
    },
    stroke: {
        show: true,
        width: 2,
        colors: ['transparent']
    },
    xaxis: {
        categories: ['Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
    },
    yaxis: {
        title: {
            text: 'Cantidad de actividades'
        }
    },
    fill: {
        opacity: 1
    },
    tooltip: {
        y: {
            /*formatter: function (val) {
                return "$ " + val + " thousands"
            }*/
        }
    }
};

//Resultados de actividades
var actividadesResultadosOptions = {
    series: [
         {
            name: "AD",
            data: [12, 9, 14]
        },
        {
            name: "A",
            data: [9, 13, 4]
        }
        ,
        {
            name: "B",
            data: [5, 6, 8]
        }
        ,
        {
            name: "C",
            data: [4, 2, 4]
        }
    ],
    colors: ['#77B6EA', '#545454', '#5d8db3', '#e3df7b' ],
    chart: {
        height: 350,
        type: 'line',
        zoom: {
            enabled: false
        }
    },
    dataLabels: {
        enabled: false
    },
    stroke: {
        curve: 'straight'
    },
    title: {
        text: 'Actividades por calificación',
        align: 'left'
    },
    grid: {
        row: {
            colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
            opacity: 0.5
        },
    },
    xaxis: {
        categories: ['Foro', 'Lectura', 'Test'],
    }
};



var actividades = new ApexCharts(document.querySelector("#actividades-mes"), actividadesOptions);
var actividadesResultados = new ApexCharts(document.querySelector("#actividades-resultados"), actividadesResultadosOptions);

actividades.render();
actividadesResultados.render();