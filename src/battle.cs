using Godot;
using System;

public partial class battle : Node
{
    int maxPlayerHP = 50, PlayerHP;
    int maxEnemyHP = 30, EnemyHP;

    int playerDMG = 20;
    int enemyDMG = 15;

    bool playerTurn = true;
    bool inBattle = true;

    Panel textBox;
    Panel actionMenu;

    TextureProgressBar playerHealthBar;
    TextureProgressBar enemyHealthBar;

    Label playerHealthText;
    Label enemyHealthText;
    public override void _Ready()
    {
        base._Ready();

        PlayerHP = maxPlayerHP;
        EnemyHP = maxEnemyHP;

        textBox = GetNode<Panel>("./HUD/TextBox");
        actionMenu = GetNode<Panel>("./HUD/Actions");

        UpdateTextBoxLabel(">A wild cube appears");

        playerHealthBar = GetNode<TextureProgressBar>("./Player/HealthBar/Bar");
        enemyHealthBar = GetNode<TextureProgressBar>("./Fighter/HealthBar/Bar");

        playerHealthBar.MaxValue = maxPlayerHP;
        playerHealthBar.Value = maxPlayerHP;

        enemyHealthBar.MaxValue = maxEnemyHP;
        enemyHealthBar.Value = maxEnemyHP;

        playerHealthText = GetNode<Label>("./Player/HealthBar/Bar/Label");
        enemyHealthText = GetNode<Label>("./Fighter/HealthBar/Bar/Label");

        playerHealthText.Text = PlayerHP + "/" + maxPlayerHP;
        enemyHealthText.Text = EnemyHP + "/" + maxEnemyHP;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        CombatCheck();
    }

    public void CombatCheck()
    {
        if (inBattle)
        {
            if (!playerTurn && actionMenu.Visible)
            {
                EnemyAttack();
            }
            if (PlayerHP <= 0)
            {
                ShowMessage();
                UpdateTextBoxLabel(">The player's vision starts to blur, losing counciousness as he falls into the ground.");
                inBattle = false;
            }
            else if (EnemyHP <= 0)
            {
                ShowMessage();
                UpdateTextBoxLabel(">The cube breaks down like glass, disapearing after a few moments.");
                inBattle = false;
            }
        }
    }

    public void PlayerAttack()
    {
        UpdateTextBoxLabel(">The player deals " + playerDMG + " damage to the red cube.");
        ShowMessage();
        EnemyHP = DealDamage(EnemyHP, playerDMG);
        enemyHealthText = UpdateHealthDisplayText(enemyHealthText, EnemyHP, maxEnemyHP);
        enemyHealthBar = UpdateHealthDisplay(enemyHealthBar, EnemyHP);
        playerTurn = false;
    }

    public void EnemyAttack()
    {
        UpdateTextBoxLabel(">The enemy deals " + enemyDMG + " damage to the player.");
        ShowMessage();
        PlayerHP = DealDamage(PlayerHP, enemyDMG);
        playerHealthText = UpdateHealthDisplayText(playerHealthText, PlayerHP, maxPlayerHP);
        playerHealthBar = UpdateHealthDisplay(playerHealthBar, PlayerHP);
        playerTurn = true;
    }

    public void UpdateTextBoxLabel(string text)
    {
        var textBoxLabel = textBox.GetChild<Label>(0);
        textBoxLabel.Text = text;
    }

    public Label UpdateHealthDisplayText(Label display, int currentHP, int maxHP)
    {
        display.Text = currentHP + "/" + maxHP;
        return display;
    }

    public TextureProgressBar UpdateHealthDisplay(TextureProgressBar display, int currentHP)
    {
        display.Value = currentHP;
        return display;
    }

    public int DealDamage(int targetHP, int damage)
    {
        targetHP -= damage;
        if(targetHP < 0)
        {
            targetHP = 0;
        }
        return targetHP;
    }

    public void ShowMessage()
    {
        actionMenu.Visible = false;
        textBox.Visible = true;
    }

    public void HideMessage()
    {
        if (inBattle)
        {
            textBox.Visible = false;
            actionMenu.Visible = true;
        }
        else
        {
            textBox.Visible = false;
        }
    }
}
