import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '1m', target: 10 },  // ramp up
        { duration: '3m', target: 10 },  // steady
        { duration: '1m', target: 0 },   // ramp down
    ],
    thresholds: {
        http_req_duration: ['p(95)<2000'],
        http_req_failed: ['rate<0.05'],
    },
};

const BASE_URL = __ENV.APP_URL || 'https://bp-prod-webapp-staging.azurewebsites.net';

export default function () {
    const res = http.get(`${BASE_URL}/`);

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

    sleep(1);
}
