import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
    stages: [
        { duration: '30s', target: 20 },  // below normal load
        { duration: '30s', target: 50 },  // normal load
        { duration: '30s', target: 100 }, // stress point
        { duration: '10s', target: 0 },   // recover
    ]
};

const BASE_URL = __ENV.BP_K6_BASEURL || 'https://bp-prod-webapp-staging.azurewebsites.net';

export default function () {
    const res = http.get(BASE_URL);

    check(res, {
        'status is 200 or 302': (r) => r.status === 200 || r.status === 302,
    });

    sleep(1);
}
