import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '20s', target: 10 }, // ramp up
        { duration: '30s', target: 10 }, // sustain load
        { duration: '10s', target: 0 },  // ramp down
    ],
    thresholds: {
        http_req_duration: ['p(95)<600'],  // 95% responses < 600ms
    },
};

const BASE_URL = __ENV.BP_K6_BASEURL || 'http://localhost:53135/';

export default function () {
    const res = http.get(BASE_URL);

    check(res, {
        'status 200': (r) => r.status === 200,
    });

    sleep(1);
}
