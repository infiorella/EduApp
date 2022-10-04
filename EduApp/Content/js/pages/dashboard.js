//CHARTS

//Avance Competencias
var optionsAvanceCompetencias = {
	annotations: {
		position: 'back'
	},
	dataLabels: {
		enabled:false
	},
	chart: {
		type: 'bar',
		height: 300
	},
	fill: {
		opacity:1
	},
	plotOptions: {
	},
	series: [{
		name: 'Alumnos',
		data: [9,20,30,20,10,20]
	}],
	colors: '#435ebe',
	xaxis: {
		categories: ["Competencia 1", "Competencia 2", "Competencia 3", "Competencia 4", "Competencia 5", "Competencia 6"],
	},
}


//Avance Notas
let optionsNotas  = {
	series: [40, 30, 10,20],
	labels: ['AD','A', 'B', 'C'],
	colors: ['#5ddab4','#435ebe', '#55c6e8','#ff7976'],
	chart: {
		type: 'donut',
		width: '100%',
		height:'350px'
	},
	legend: {
		position: 'bottom'
	},
	plotOptions: {
		pie: {
			donut: {
				size: '30%'
			}
		}
	}
}


//Avance de Actividades
var optionsForo = {
	series: [{
		name: 'series1',
		data: [310, 800, 600, 430, 540, 340, 605, 805,430, 540, 340, 605]
	}],
	chart: {
		height: 80,
		type: 'area',
		toolbar: {
			show:false,
		},
	},
	colors: ['#5350e9'],
	stroke: {
		width: 2,
	},
	grid: {
		show:false,
	},
	dataLabels: {
		enabled: false
	},
	xaxis: {
		type: 'datetime',
		categories: ["2018-09-19T00:00:00.000Z", "2018-09-19T01:30:00.000Z", "2018-09-19T02:30:00.000Z", "2018-09-19T03:30:00.000Z", "2018-09-19T04:30:00.000Z", "2018-09-19T05:30:00.000Z", "2018-09-19T06:30:00.000Z","2018-09-19T07:30:00.000Z","2018-09-19T08:30:00.000Z","2018-09-19T09:30:00.000Z","2018-09-19T10:30:00.000Z","2018-09-19T11:30:00.000Z"],
		axisBorder: {
			show:false
		},
		axisTicks: {
			show:false
		},
		labels: {
			show:false,
		}
	},
	show:false,
	yaxis: {
		labels: {
			show:false,
		},
	},
	tooltip: {
		x: {
			format: 'dd/MM/yy HH:mm'
		},
	},
};

let optionsLectura = {
	...optionsForo,
	colors: ['#008b75'],
}
let optionsTest = {
	...optionsForo,
	colors: ['#dc3545'],
}



//AVANCE DE COMPETENCIAS
var chartAvanceCompetencias = new ApexCharts(document.querySelector("#chart-avance-competencias"), optionsAvanceCompetencias);

chartAvanceCompetencias.render();


//Avance de Actividades
var chartForo = new ApexCharts(document.querySelector("#chart-foro"), optionsForo);
var chartLectura = new ApexCharts(document.querySelector("#chart-lectura"), optionsLectura);
var chartTest = new ApexCharts(document.querySelector("#chart-test"), optionsTest);

chartTest.render();
chartLectura.render();
chartForo.render();

//Avance de Notas
var chartNotas = new ApexCharts(document.getElementById('chart-notas'), optionsNotas)
chartNotas.render()