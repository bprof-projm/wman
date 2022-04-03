import axios from "axios";
import { useEffect, useState } from "react";
import { SearchBox } from "../../SearchBox/search-box.component";
import MyCard from "../../User-card/mycard.component";
import './manage-workforce.styles.css'

const ManageWorkforce = () => {
    const [updateUser, setUpdateUser] = useState('')
    const [users, setUsers] = useState([]);
    const [searchField, setSearchField] = useState("");
    // Search specific user
    const handleChange = (e) => {
        setSearchField(e.target.value);
    };
    const filteredUsers = users.filter((user) =>
        user.lastname.toLowerCase().includes(searchField.toLowerCase())
    );

    useEffect(() => {
        axios.get("/Auth/all")
            .then((response) => setUsers(response.data))
            .catch((error) => console.log(error))
    }, []);
    return (
        <div className="manage-workforce">
            <div className="admin-workforce-operations"><h1>opsd</h1></div>
            <div className="admin-list-of-users">
                <div className="search-admin"><SearchBox
                    placeholder="Type the lastname of the user"
                    handleChange={handleChange}
                /></div>

                <div className="users-list">
                    {filteredUsers.map((user, index) => (
                        <MyCard key={index} object={user} src={user.picture?.url} func={() => setUpdateUser} />
                    ))}
                </div>
            </div>
        </div>
    )
}

export default ManageWorkforce;