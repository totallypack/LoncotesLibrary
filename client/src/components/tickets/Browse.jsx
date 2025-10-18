import { useEffect, useState } from "react";
import { Table, Button } from "reactstrap";
import { getAvailableMaterials } from "../../data/materialsData";
import { useNavigate } from "react-router-dom";

export default function Browse() {
  const [materials, setMaterials] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    getAvailableMaterials().then(setMaterials);
  }, []);

  return (
    <div className="container">
      <div className="sub-menu bg-light">
        <h4>Browse Available Materials</h4>
      </div>
      <Table>
        <thead>
          <tr>
            <th>Id</th>
            <th>Title</th>
            <th>Type</th>
            <th>Genre</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {materials.map((m) => (
            <tr key={`material-${m.id}`}>
              <th scope="row">{m.id}</th>
              <td>{m.materialName}</td>
              <td>{m.materialType.name}</td>
              <td>{m.genre.name}</td>
              <td>
                <Button
                  color="primary"
                  size="sm"
                  onClick={() => navigate(`/materials/${m.id}/checkout`)}
                >
                  Check out
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
}
