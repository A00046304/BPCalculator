import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 1,
    duration: '30s',
    thresholds: {
        http_req_duration: ['p(95)<2000'],
        http_req_failed: ['rate<0.10'],
    },
};

const BASE_URL = __ENV.APP_URL || 'https://bp-prod-webapp-staging.azurewebsites.net';

export default function () {
    const res = http.get(`${BASE_URL}/`);

    check(res, {
        'status is 200': (r) => r.status === 200,
        'html has title text': (r) =>
            String(r.body).toLowerCase().includes('blood pressure'),
    });

    sleep(1);
}
