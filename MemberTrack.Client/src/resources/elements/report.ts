import { PersonReportDto } from "../../core/dtos";
import { bindable, customElement } from "aurelia-framework";
import * as Chart from "chart.js";

@customElement("mt-report")
export class Report {
    @bindable data: PersonReportDto = null;
    private element: Element;

    constructor(element: Element) {
        this.element = element;
    }

    public dataChanged(newValue: PersonReportDto): void {
        if (!newValue) {
            return;
        }

        let ctx = (this.element.querySelector("canvas") as any).getContext("2d");

        new Chart(ctx, {
            type: "bar",
            data: {
                labels: this.data.items.map(item => item.monthName),
                datasets: [
                    {
                        label: "Members",
                        data: this.data.items.map(item => item.memberCount),
                        borderWidth: 1,
                        fill: false,
                        lineTension: 0.1,
                        backgroundColor: "rgba(9, 98, 241, 0.4)",
                        borderColor: "rgba(9, 98, 241,1)",
                        borderCapStyle: "butt",
                        borderDash: [],
                        borderDashOffset: 0.0,
                        borderJoinStyle: "miter",
                        pointBorderColor: "rgba(9, 98, 241,1)",
                        pointBackgroundColor: "#fff",
                        pointBorderWidth: 1,
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(9, 98, 241,1)",
                        pointHoverBorderColor: "rgba(220,220,220,1)",
                        pointHoverBorderWidth: 2,
                        pointRadius: 1,
                        pointHitRadius: 10,
                        spanGaps: false
                    },
                    {
                        label: "Visitors",
                        data: this.data.items.map(item => item.visitorCount),
                        borderWidth: 1,
                        fill: false,
                        lineTension: 0.1,
                        backgroundColor: "rgba(203,203,108,0.4)",
                        borderColor: "rgba(203,203,108,1)",
                        borderCapStyle: "butt",
                        borderDash: [],
                        borderDashOffset: 0.0,
                        borderJoinStyle: "miter",
                        pointBorderColor: "rgba(203,203,108,1)",
                        pointBackgroundColor: "#fff",
                        pointBorderWidth: 1,
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(203,203,108,1)",
                        pointHoverBorderColor: "rgba(220,220,220,1)",
                        pointHoverBorderWidth: 2,
                        pointRadius: 1,
                        pointHitRadius: 10,
                        spanGaps: false
                    }
                ]
            },
            options: {
                title: {
                    display: true,
                    text: "Member / Visitor Report",
                    fontSize: 16
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                legend: {
                    display: true
                },
                responsive: true
            }
        });
    }
}