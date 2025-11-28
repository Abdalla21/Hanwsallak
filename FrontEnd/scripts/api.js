// API Service Module for Hanwsallak
const API_BASE_URL = 'https://localhost:7241/api'; // Update this to match your backend URL (check launchSettings.json)

// Get authentication token from localStorage
function getAuthToken() {
    return localStorage.getItem('authToken') || localStorage.getItem('jwtToken') || '';
}

// Set authentication token in localStorage
function setAuthToken(token) {
    localStorage.setItem('authToken', token);
    localStorage.setItem('jwtToken', token); // Also store as jwtToken for compatibility
}

// Remove authentication token
function removeAuthToken() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('jwtToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('userInfo');
}

// Generic fetch wrapper with error handling
async function apiRequest(endpoint, options = {}) {
    const token = getAuthToken();
    const headers = {
        'Content-Type': 'application/json',
        ...options.headers
    };

    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            ...options,
            headers
        });

        if (!response.ok) {
            let errorData;
            try {
                errorData = await response.json();
            } catch (e) {
                errorData = { message: `HTTP error! status: ${response.status}` };
            }
            
            // Create error object with response data
            const error = new Error(errorData.message || `HTTP error! status: ${response.status}`);
            error.response = response;
            error.status = response.status;
            error.data = errorData;
            throw error;
        }

        return await response.json();
    } catch (error) {
        console.error('API Request Error:', error);
        // If it's already our custom error, re-throw it
        if (error.response) {
            throw error;
        }
        // Otherwise, wrap it
        const wrappedError = new Error(error.message || 'حدث خطأ في الاتصال بالخادم');
        throw wrappedError;
    }
}

// Trip API Methods
const TripAPI = {
    create: async (tripData) => {
        return apiRequest('/Trip/Create', {
            method: 'POST',
            body: JSON.stringify(tripData)
        });
    },

    getMyTrips: async () => {
        return apiRequest('/Trip/MyTrips');
    },

    getAvailable: async (fromCity = null, toCity = null) => {
        const params = new URLSearchParams();
        if (fromCity) params.append('fromCity', fromCity);
        if (toCity) params.append('toCity', toCity);
        const queryString = params.toString();
        return apiRequest(`/Trip/Available${queryString ? '?' + queryString : ''}`);
    },

    update: async (id, tripData) => {
        return apiRequest(`/Trip/${id}`, {
            method: 'PUT',
            body: JSON.stringify(tripData)
        });
    },

    delete: async (id) => {
        return apiRequest(`/Trip/${id}`, {
            method: 'DELETE'
        });
    }
};

// Shipment API Methods
const ShipmentAPI = {
    create: async (shipmentData) => {
        return apiRequest('/Shipment/Create', {
            method: 'POST',
            body: JSON.stringify(shipmentData)
        });
    },

    getMyShipments: async () => {
        return apiRequest('/Shipment/MyShipments');
    },

    getAvailable: async (fromCity = null, toCity = null) => {
        const params = new URLSearchParams();
        if (fromCity) params.append('fromCity', fromCity);
        if (toCity) params.append('toCity', toCity);
        const queryString = params.toString();
        return apiRequest(`/Shipment/Available${queryString ? '?' + queryString : ''}`);
    },

    delete: async (id) => {
        return apiRequest(`/Shipment/${id}`, {
            method: 'DELETE'
        });
    }
};

// Matching API Methods
const MatchingAPI = {
    getShipmentsForTrip: async (tripId) => {
        return apiRequest(`/Matching/ShipmentsForTrip/${tripId}`);
    },

    getTripsForShipment: async (shipmentId) => {
        return apiRequest(`/Matching/TripsForShipment/${shipmentId}`);
    }
};

// Order API Methods
const OrderAPI = {
    create: async (orderData) => {
        return apiRequest('/Order/Create', {
            method: 'POST',
            body: JSON.stringify(orderData)
        });
    },

    getMyOrders: async () => {
        return apiRequest('/Order/MyOrders');
    },

    accept: async (orderId) => {
        return apiRequest(`/Order/${orderId}/Accept`, {
            method: 'PUT'
        });
    },

    startDelivery: async (orderId) => {
        return apiRequest(`/Order/${orderId}/StartDelivery`, {
            method: 'PUT'
        });
    },

    markDelivered: async (orderId) => {
        return apiRequest(`/Order/${orderId}/MarkDelivered`, {
            method: 'PUT'
        });
    }
};

// Authentication API Methods
const AuthAPI = {
    login: async (email, password) => {
        const response = await apiRequest('/Authentication/Login', {
            method: 'POST',
            body: JSON.stringify({ email, password })
        });
        if (response.token) {
            setAuthToken(response.token);
            // Store refresh token if provided
            if (response.refreshToken) {
                localStorage.setItem('refreshToken', response.refreshToken);
            }
            // Store user info if provided
            if (response.user) {
                localStorage.setItem('userInfo', JSON.stringify(response.user));
            }
        }
        return response;
    },

    register: async (registerData) => {
        return apiRequest('/Authentication/Register', {
            method: 'POST',
            body: JSON.stringify(registerData),
            authRequired: false
        });
    },

    confirmEmail: async (userId, confirmationToken) => {
        return apiRequest('/Authentication/ConfirmEmail', {
            method: 'POST',
            body: JSON.stringify({ userID: userId, confirmationToken })
        });
    },

    forgotPassword: async (email) => {
        return apiRequest('/Authentication/ForgotPassword', {
            method: 'POST',
            body: JSON.stringify(email)
        });
    },

    resetPassword: async (email, resetToken, newPassword) => {
        return apiRequest('/Authentication/ResetPassword', {
            method: 'POST',
            body: JSON.stringify({ email, resetToken, newPassword })
        });
    },

    resendConfirmationEmail: async (email) => {
        return apiRequest('/Authentication/ResendConfirmationEmail', {
            method: 'POST',
            body: JSON.stringify(email)
        });
    },

    logout: () => {
        removeAuthToken();
        localStorage.removeItem('refreshToken');
    }
};

// Export API methods
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { TripAPI, ShipmentAPI, MatchingAPI, OrderAPI, AuthAPI, getAuthToken, setAuthToken, removeAuthToken };
}

