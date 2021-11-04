import { Button, Drawer, Radio, Space } from "antd";
import React, { useEffect, useState } from "react";
import { PieChartOutlined } from '@ant-design/icons';
import ProgressList from "../Progress-list/progress-list.component";
import axios from "axios";
import { SearchBox } from "../../SearchBox/search-box.component";
import './progress-menu.styles.css';


const ProgressMenu = () => {
    /*Visibility*/
    const [visible, setVisible] = useState(false);

    const showMenu = () => {
        setVisible(true);
    }

    const closeMenu = () => {
        setVisible(false);
    }
    /*Placement*/
    const [placement, setPlacement] = useState('right')

    const changePlacement = e => {
        setPlacement(e.target.value);
    };
    /*Get elements*/
    const [users, setUsers] = useState([]);
    useEffect(() => {
        axios.get(`https://mocki.io/v1/c6f3f270-21ea-450c-9428-715ec0babc6d`)
            .then(response => setUsers(response.data));
    }, [axios]);

    /*Search user by name*/
    const [searchField, setSearchField] = useState("");

    const handleChange = (e) => {
        setSearchField(e.target.value);
    }

    const filteredUsers =
        users.filter(
            user => user.name.toLowerCase().includes(searchField.toLowerCase())
        );


    return (
        <div>
            <Button shape="circle" icon={<PieChartOutlined />} onClick={showMenu} />

            <Drawer title="Employees' workload" placement={placement} onClose={closeMenu} visible={visible}>

                <details>
                    <summary>Placement Options</summary>
                    <div>
                        <Space>
                            <Radio.Group value={placement} onChange={changePlacement}>
                                <Radio value="left">left</Radio>
                                <Radio value="right">right</Radio>
                            </Radio.Group>
                        </Space></div>
                </details>
                <br />
                <div className="searchbox">
                    <SearchBox placeholder="Search User" handleChange={handleChange} />
                </div>
                <br />
                {/*props needed objects(list)*/}
                <div>
                    <ProgressList objects={filteredUsers} />
                </div>

            </Drawer>

        </div>
    )
}

export default ProgressMenu;