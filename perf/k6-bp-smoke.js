import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    stages: [
        { duration: '10s', target: 10 },   // ramp up
        { duration: '20s', target: 10 },   // steady
        { duration: '10s', target: 0 },    // ramp down
    ],
    thresholds: {
        http_req_duration: ['p(95)<2000'], //
        http_req_failed: ['rate<0.10'],   //
    },
};

const BASE_URL = __ENV.APP_URL || 'https://bp-prod-webapp-staging.azurewebsites.net';

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
