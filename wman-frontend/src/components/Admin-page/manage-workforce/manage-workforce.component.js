import { Button } from "antd";
import axios from "axios";
import { useEffect, useState } from "react";
import Swal from "sweetalert2";
import { SearchBox } from "../../SearchBox/search-box.component";
import MyCard from "../../User-card/mycard.component";
import AddWorkforce from "./add-workforce.component";
import './manage-workforce.styles.css'
import UpdateWorkforce from "./update-workforce.component";

const ManageWorkforce = () => {
    const [users, setUsers] = useState([]);
    const [searchField, setSearchField] = useState("");
    const [selected, setSelected] = useState('');
    const [update, setUpdate] = useState(false);

    // Search specific user
    const handleChange = (e) => {
        setSearchField(e.target.value);
    };

    const filteredUsers = users.filter((user) =>
        user.lastname.toLowerCase().includes(searchField.toLowerCase())
    );
    const handleUpdate = () => {
        setUpdate(!update);
    }
    const handleDelete = () => {
        axios.delete(`/Admin/Delete?username=${selected.username}`)
            .then((response) => {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'User Deleted!',
                })
                handleUpdate();
                setSelected('');
            })
            .catch((error) => Swal.fire({
                icon: 'error',
                title: 'Oops!',
                text: 'User not deleted!',
            }))
    }

    useEffect(() => {
        axios.get("/Auth/all")
            .then((response) => setUsers(response.data))
            .catch((error) => console.log(error))
    }, [update]);
    return (
        <div className="manage-workforce">
            <div className="admin-workforce-information">
                <div className="admin-workforce-information">
                    <h1>Selected User:</h1>
                    {selected === ''
                        ? <h2>No user selected</h2>
                        : <div className="admin-workforce-information">
                            <MyCard key={selected.lastname} object={selected} func={() => ''} />
                            <Button onClick={() => setSelected('')}>Deselect</Button>
                        </div>}
                </div>
                <br />
                {selected === ''
                    ? null
                    : <Button onClick={handleDelete}>Delete</Button>}
            </div>
            <div className="admin-workforce-operations">
                {selected === ''
                    ?
                    <AddWorkforce func={handleUpdate} />
                    : <UpdateWorkforce object={selected} func={handleUpdate} />}
            </div>

            <div className="admin-list-of-users">
                <div className="search-admin"><SearchBox
                    placeholder="Type the lastname of the user"
                    handleChange={handleChange}
                />
                </div>

                <div className="users-list">
                    {filteredUsers.map((user, index) => (
                        <MyCard key={index} object={user} func={setSelected} src={user.picture?.url} />
                    ))}
                </div>
            </div>
        </div>
    )
}

export default ManageWorkforce;