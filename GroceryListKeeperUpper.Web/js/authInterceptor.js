//interceptor to make sure our token goes out on all requests.
app.factory('authInterceptor', function ($rootScope, $q, $window, $location) {
    return {

        request: function (config) {
            config.headers = config.headers || {};
            if ($window.sessionStorage.getItem('token')) {
                config.headers.Authorization = 'Token ' + $window.sessionStorage.getItem('token');
            } else {
                $window.sessionStorage.clear();
                $location.path('/login');
            }
            return config;
        },

        //department of redundency department.
        response: function (response) {
            if (response.status === 401) {
                $window.sessionStorage.clear();
                $location.path('/login');
            }
            return response || $q.when(response);
        },

        //department of redundency department.
        responseError: function (response) {
            if (response.status === 401) {
                $window.sessionStorage.clear();
                $location.path('/login');
            }
            return $q.reject(response);
        }
    };
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
});