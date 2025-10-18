const _apiUrl = "/api/patrons";

export const getPatrons = () => {
  return fetch(_apiUrl).then((r) => r.json());
};

export const getPatron = (id) => {
  return fetch(`${_apiUrl}/${id}`).then((r) => r.json());
};

export const updatePatron = (id, patron) => {
  return fetch(`${_apiUrl}/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(patron),
  });
};

export const deactivatePatron = (id) => {
  return fetch(`${_apiUrl}/${id}/deactivate`, {
    method: "PUT",
  });
};
