import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 2,
    duration: '40s',
    thresholds: {
        // 95% of requests under 2s
        http_req_duration: ['p(95)<2000'],
        // allow up to 10% failures (tweak later if you want stricter)
        http_req_failed: ['rate<0.10'],
    },
};

// Use APP_URL from the pipeline; fallback for local runs
const BASE_URL = __ENV.APP_URL || 'https://bp-prod-webapp-staging.azurewebsites.net';

export default function () {
    const res = http.get(`${BASE_URL}/`);

    check(res, {
        'status is 200': (r) => r.status === 200,
        "page contains 'Blood Pressure'": (r) =>
            String(r.body).includes('Blood Pressure'),
    });

    sleep(1);
}
