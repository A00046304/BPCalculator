import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 1,
    duration: '10s',
};

const BASE_URL = __ENV.APP_URL || 'https://bp-prod-webapp-staging.azurewebsites.net';

export default function () {
    const res = http.get(BASE_URL);

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

    sleep(1);
}
