//Cantidad de acitvidades completadas or mes
var actividadesOptions = {
    series: [{
        name: 'Foro',
        data: [44, 55, 57, 56, 61, 58, 63, 60, 66]
    }, {
        name: 'Test',
        data: [76, 85, 101, 98, 87, 105, 91, 114, 94]
    }, {
        name: 'Lectura',
        data: [35, 41, 36, 26, 45, 48, 52, 53, 41]
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
        categories: ['Marzo', 'Abrl', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre'],
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
            name: "A",
            data: [28, 29, 33]
        },
        {
            name: "B",
            data: [12, 11, 14]
        }
    ],
    colors: ['#77B6EA', '#545454'],
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