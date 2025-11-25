import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    stages: [
        { duration: '10s', target: 10 },   // ramp up
        { duration: '20s', target: 10 },   // steady
        { duration: '10s', target: 0 },    // ramp down
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'], // 95% must be < 500ms
        http_req_failed: ['rate<0.01'],   // <1% failures
    },
};

const BASE_URL = "https://bp-staging-webapp.azurewebsites.net";

export default function () {

    let res = http.post(`${BASE_URL}`, {
        "BP.Systolic": "130",
        "BP.Diastolic": "85"
    });

    check(res, {
        "status is 200": (r) => r.status === 200,
        "contains PreHigh": (r) => r.body.includes("PreHigh")
    });

    sleep(1);
}
