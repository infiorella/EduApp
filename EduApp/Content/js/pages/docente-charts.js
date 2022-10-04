//Avance Competencias
var avanceOptions = {
  chart: {
    type: "line",
  },
  series: [
    {
      name: "Alumnos",
      data: [5, 10, 15, 20, 25, 30],
    },
  ],
  xaxis: {
    categories: ["Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto"],
  },
};

//Progreso de notas por mes
var progresoOptions = {
    series: [{
        name: 'AD',
        data: [44, 55, 41, 67, 22, 43]
    }, {
        name: 'A',
        data: [13, 23, 20, 8, 13, 27]
    }, {
        name: 'B',
        data: [11, 17, 15, 15, 21, 14]
    }, {
        name: 'C',
        data: [21, 7, 25, 13, 22, 8]
    }],
    chart: {
        type: 'bar',
        height: 350,
        stacked: true,
        toolbar: {
            show: true
        },
        zoom: {
            enabled: true
        }
    },
    responsive: [{
        breakpoint: 480,
        options: {
            legend: {
                position: 'bottom',
                offsetX: -10,
                offsetY: 0
            }
        }
    }],
    plotOptions: {
        bar: {
            horizontal: false,
            borderRadius: 10
        },
    },
    xaxis: {
        categories: ['Marzo', 'Abril', 'Mayo', 'Junio',
            'Julio', 'Agosto'
        ],
    },
    legend: {
        position: 'right',
        offsetY: 40
    },
    fill: {
        opacity: 1
    }
};

//actividades-desarrolladas
var actividadesOptions = {
   series: [
     {
       name: "Foro",
       data: [31, 40, 28, 51, 42, 109, 100],
     },
     {
       name: "Test",
       data: [11, 32, 45, 32, 34, 52, 41],
     },
     {
       name: "Lectura",
       data: [5, 43, 15, 40, 39, 22, 61],
     },
   ],
   chart: {
     height: 350,
     type: "area",
   },
   dataLabels: {
     enabled: false,
   },
   stroke: {
     curve: "smooth",
   },
   xaxis: {
     categories: [
       "Enero",
       "Febrero",
       "Marzo",
       "Abril",
       "Mayo",
       "Junio",
       "Julio",
     ],
   },
   tooltip: {     
   },
};

//Satisfacción de estudiantes
var satisfaccionOptions = {
    series: [44, 55, 13],
    chart: {
        width: 330,
        type: 'pie',
    },
    legend: {
        position: 'bottom'
    },
    labels: ['Foro', 'Test', 'Lectura'],
    responsive: [{
        breakpoint: 480,
        options: {
            chart: {
                width: 200
            }
        },
    }]
};


var avance = new ApexCharts(document.querySelector("#avance-competencias"), avanceOptions);
var progreso = new ApexCharts(document.querySelector("#progreso-notas"), progresoOptions);
var actividades = new ApexCharts(document.querySelector("#actividades-desarrolladas"), actividadesOptions);
var satisfaccion = new ApexCharts(document.querySelector("#satisfaccion-actividad"), satisfaccionOptions);


satisfaccion.render();
progreso.render();
actividades.render();
avance.render();
