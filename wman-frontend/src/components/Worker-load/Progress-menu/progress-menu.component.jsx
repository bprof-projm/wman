import { Button, Drawer, Radio, Space } from "antd";
import React, { useEffect, useState } from "react";
import { PieChartOutlined } from "@ant-design/icons";
import ProgressList from "../Progress-list/progress-list.component";
import axios from "axios";
import { SearchBox } from "../../SearchBox/search-box.component";
import "./progress-menu.styles.css";
import Cookies from "js-cookie";
const ProgressMenu = () => {
  /*Visibility*/
  const [visible, setVisible] = useState(false);

  const showMenu = () => {
    setVisible(true);
  };

  const closeMenu = () => {
    setVisible(false);
  };

  /*Placement*/
  const [placement, setPlacement] = useState("right");

  const changePlacement = (e) => {
    setPlacement(e.target.value);
  };

  /*Get elements*/
  const [users, setUsers] = useState([]);
  useEffect(() => {
    axios
      .get(`/User/workload`, {
        headers: { Authorization: `Bearer ${Cookies.get("auth")}` },
      })
      .then((response) => setUsers(response.data))
      .catch((error) => console.log(error));
  }, [axios]);

  /*Search user by name*/
  const [searchField, setSearchField] = useState("");

  const handleChange = (e) => {
    setSearchField(e.target.value);
  };

  const filteredUsers = users.filter((user) =>
    user.username.toLowerCase().includes(searchField.toLowerCase())
  );

  console.log(filteredUsers);

  return (
    <div>
      <Button type="text" icon={<PieChartOutlined />} onClick={showMenu}>
        Workload
      </Button>

      <Drawer
        title="Employees' workload"
        placement={placement}
        onClose={closeMenu}
        visible={visible}
        width={350}
      >
        <details>
          <summary>Placement Options</summary>
          <div>
            <Space>
              <Radio.Group value={placement} onChange={changePlacement}>
                <Radio value="left">left</Radio>
                <Radio value="right">right</Radio>
              </Radio.Group>
            </Space>
          </div>
        </details>
        <br />
        <div className="searchbox">
          <SearchBox placeholder="Search User" handleChange={handleChange} />
        </div>

        {/*props needed objects(list)*/}
        <div>
          <ProgressList objects={filteredUsers} />
        </div>
      </Drawer>
    </div>
  );
};

export default ProgressMenu;
