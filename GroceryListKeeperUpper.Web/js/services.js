appServices.factory('listService', function($http){
	return {
		getAllLists: function(){
			return $http.get(options.api.base_url + '/lists');
		},

        addList: function(listname) {
            return $http.post(options.api.base_url + '/lists/create', listname);
        },

		getList: function(listid){
			return $http.get(options.api.base_url + '/lists/' + listid);
		},

        deleteList: function(listid) {
            return $http.delete(options.api.base_url + '/lists/' + listid);
        },

        addItem: function(listid, item) {
            return $http.post(options.api.base_url + '/lists/' + listid + '/items', item);
        },

        updateItem: function(listid, item) {
            return $http.put(options.api.base_url + '/lists/' + listid + '/item/' + item.id, item);
        },

        deleteItem: function (listid, itemid) {
            return $http.delete(options.api.base_url + '/lists/' + listid + '/items/' + itemid);
        }

	};
});

appServices.factory('userService', function($http){
	return {
		login: function(user){
			return $http.post(options.api.base_url + '/auth', user);
		},
        register: function(user) {
            return $http.post(options.api.base_url + '/users', user);
        }
	}
});

appServices.factory('sessionService', function($window) {
    return {
        store: function(name, item) {
            return $window.sessionStorage.setItem(name, item);
        },
        get: function(name) {
            return $window.sessionStorage.getItem(name);
        },
        delete: function(name) {
            return $window.sessionStorage.removeItem(name);
        },
        clear: function() {
            return $window.sessionStorage.clear();
        }
    };
});

appServices.factory('itemService', function($http){
	return {
		addItem: function(item){
			return $http.post(options.api.base_url + '/items/addItem', item);
		}
	}
});